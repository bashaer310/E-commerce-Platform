using AutoMapper;
using Backend_Teamwork.src.Entities;
using Backend_Teamwork.src.Repository;
using Backend_Teamwork.src.Utils;
using static Backend_Teamwork.src.DTO.BookingDTO;
using static Backend_Teamwork.src.Entities.User;

namespace Backend_Teamwork.src.Services.booking
{
    public class BookingService : IBookingService
    {
        private readonly BookingRepository _bookingRepository;
        private readonly WorkshopRepository _workshopRepository;

        private readonly IMapper _mapper;

        public BookingService(
            BookingRepository bookingRepository,
            WorkshopRepository workshopRepository,
            IMapper mapper
        )
        {
            _bookingRepository = bookingRepository;
            _workshopRepository = workshopRepository;
            _mapper = mapper;
        }

        public async Task<List<BookingReadDto>> GetAllAsync(PaginationOptions paginationOptions)
        {
            var bookings = await _bookingRepository.GetAllAsync(paginationOptions);
            if (bookings.Count == 0)
            {
                throw CustomException.NotFound($"Bookings not found");
            }
            return _mapper.Map<List<Booking>, List<BookingReadDto>>(bookings);
        }

        public async Task<List<BookingReadDto>> GetAllByUserIdAsync(
            PaginationOptions paginationOptions,
            Guid userId
        )
        {
            var bookings = await _bookingRepository.GetByUserIdAsync(userId, paginationOptions);
            if (bookings.Count == 0)
            {
                throw CustomException.NotFound(
                    $"Bookings associated with userId: {userId} not found"
                );
            }
            return _mapper.Map<List<Booking>, List<BookingReadDto>>(bookings);
        }

        public async Task<BookingReadDto> GetByIdAsync(Guid id, Guid userId, string userRole)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null)
            {
                throw CustomException.NotFound($"Booking with id: {id} not found");
            }
            if (userRole != UserRole.Admin.ToString() && booking.UserId != userId)
            {
                throw CustomException.Forbidden($"Not allowed to access booking with id: {id}");
            }
            return _mapper.Map<Booking, BookingReadDto>(booking);
        }

        public async Task<int> GetCountAsync()
        {
            return await _bookingRepository.GetCountAsync();
        }

        public async Task<int> GetCountByUserIdAsync(Guid id)
        {
            return await _bookingRepository.GetCountByUserIdAsync(id);
        }

        public async Task<BookingReadDto> CreateAsync(BookingCreateDto booking, Guid userId)
        {
            //1. check if the workshop is existed
            var workshop = await _workshopRepository.GetByIdAsync(booking.WorkshopId);
            if (workshop == null)
            {
                throw CustomException.NotFound($"Workshp with id: {booking.WorkshopId} not found");
            }
            //2. check if the workshop isn't available
            if (!workshop.Availability)
            {
                throw CustomException.BadRequest($"Invalid booking");
            }
            //3. check if the user already enrolled in this workshop
            bool isFound = await _bookingRepository.GetByUserIdAndWorkshopIdAsync(
                userId,
                booking.WorkshopId
            );
            if (isFound)
            {
                throw CustomException.BadRequest($"Invalid booking");
            }
            //4. check if the user enrolled in other workshop at the same time
            var workshops = await _workshopRepository.GetAllAsync(new PaginationOptions());
            var foundWorkshop = workshops.FirstOrDefault(w =>
                (w.StartTime == workshop.StartTime && w.EndTime == workshop.EndTime)
                || (w.StartTime < workshop.StartTime && w.EndTime > workshop.StartTime)
                || (w.StartTime < workshop.EndTime && w.EndTime > workshop.EndTime)
            );
            var isFound2 = false;
            if (foundWorkshop != null)
            {
                isFound2 = await _bookingRepository.GetByUserIdAndWorkshopIdAsync(
                    userId,
                    foundWorkshop.Id
                );
            }
            if (isFound2)
            {
                throw CustomException.BadRequest($"Invalid booking");
            }
            //create booking
            var mappedBooking = _mapper.Map<BookingCreateDto, Booking>(booking);
            mappedBooking.UserId = userId;
            mappedBooking.Status = BookingStatus.InProgress;
            var createdBooking = await _bookingRepository.CreateAsync(mappedBooking);
            return _mapper.Map<Booking, BookingReadDto>(createdBooking);
        }

        public async Task<BookingReadDto> UpdateStatusToConfirmedAsync(Guid id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null)
            {
                throw CustomException.NotFound($"Booking with id: {id} not found");
            }
            //1. check if the workshop isn't available
            if (!booking.Workshop.Availability)
            {
                throw CustomException.BadRequest($"Invalid updating Status");
            }
            //2. check if the booking status isn't pending
            if (booking.Status.ToString() != BookingStatus.InProgress.ToString())
            {
                throw CustomException.BadRequest($"Invalid updating Status");
            }

            //confirm booking
            booking.Status = BookingStatus.Confirmed;
            var updatedBooking = await _bookingRepository.UpdateAsync(booking);
            return _mapper.Map<Booking, BookingReadDto>(updatedBooking);
        }

        public async Task<BookingReadDto> UpdateStatusToCanceledAsync(Guid id, Guid userId)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null)
            {
                throw CustomException.NotFound($"Booking with id: {id} not found");
            }
            //1. check if the booking belongs to the user
            if (booking.UserId != userId)
            {
                throw CustomException.BadRequest($"Invalid updating Status");
            }
            //2. check if the workshop is available
            if (!booking.Workshop.Availability)
            {
                throw CustomException.BadRequest($"Invalid updating Status");
            }
            //3. check if the booking status isn't InProgress
            if (booking.Status.ToString() != BookingStatus.InProgress.ToString())
            {
                throw CustomException.BadRequest($"Invalid updating Status");
            }
            //Cancel booking
            booking.Status = BookingStatus.Canceled;
            var updatedBooking = await _bookingRepository.UpdateAsync(booking);
            return _mapper.Map<Booking, BookingReadDto>(updatedBooking);
        }
    }
}
