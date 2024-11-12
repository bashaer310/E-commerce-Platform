using AutoMapper;
using Backend_Teamwork.src.Entities;
using Backend_Teamwork.src.Repository;
using Backend_Teamwork.src.Utils;
using static Backend_Teamwork.src.DTO.OrderDTO;

namespace Backend_Teamwork.src.Services.order
{
    public class OrderService : IOrderService
    {
        private readonly OrderRepository _orderRepository;
        private readonly IMapper _mapper;

        private readonly ArtworkRepository _artworkRepository;

        public OrderService(
            OrderRepository OrderRepository,
            IMapper mapper,
            ArtworkRepository artworkRepository
        )
        {
            _orderRepository = OrderRepository;
            _mapper = mapper;
            _artworkRepository = artworkRepository;
        }

        public async Task<List<OrderReadDto>> GetAllAsync(PaginationOptions paginationOptions)
        {
            // Validate pagination options
            if (paginationOptions.PageSize <= 0)
            {
                throw CustomException.BadRequest("Page Size should be greater than 0.");
            }

            if (paginationOptions.PageNumber <= 0)
            {
                throw CustomException.BadRequest("Page Number should be 0 or greater.");
            }
            var OrderList = await _orderRepository.GetAllAsync(paginationOptions);
            if (OrderList == null)
            {
                throw CustomException.NotFound("No orders found");
            }
            return _mapper.Map<List<Order>, List<OrderReadDto>>(OrderList);
        }

        public async Task<List<OrderReadDto>> GetAllByUserIdAsync(
            PaginationOptions paginationOptions,
            Guid id
        )
        {
            // Validate pagination options
            if (paginationOptions.PageSize <= 0)
            {
                throw CustomException.BadRequest("Page Size should be greater than 0.");
            }

            if (paginationOptions.PageNumber <= 0)
            {
                throw CustomException.BadRequest("Page Number should be 0 or greater.");
            }

            var orders = await _orderRepository.GetByUserAsync(paginationOptions, id);
            if (orders == null)
            {
                throw CustomException.NotFound($"No orders found for user with id: {id}");
            }
            return _mapper.Map<List<Order>, List<OrderReadDto>>(orders);
        }

        public async Task<OrderReadDto> GetByIdAsync(Guid id)
        {
            var foundOrder = await _orderRepository.GetByIdAsync(id);
            if (foundOrder == null)
            {
                throw CustomException.NotFound($"Order with ID {id} not found.");
            }
            return _mapper.Map<Order, OrderReadDto>(foundOrder);
        }

        public async Task<int> GetCountAsync()
        {
            return await _orderRepository.GetCountAsync();
        }

        public async Task<int> GetCountByUserIdAsync(Guid id)
        {
            return await _orderRepository.GetCountByUserIdAsync(id);
        }

        public async Task<OrderReadDto> CreateOneAsync(Guid userId, OrderCreateDto createDto)
        {
            decimal totalAmount = 0;

            foreach (var orderDetail in createDto.OrderDetails)
            {
                var artwork = await _artworkRepository.GetByIdAsync(orderDetail.ArtworkId);

                // Validate if the artwork exists
                if (artwork == null)
                {
                    throw CustomException.NotFound(
                        $"Artwork with ID: {orderDetail.ArtworkId} not found."
                    );
                }

                // Check if there is enough stock for the requested quantity
                if (artwork.Quantity < orderDetail.Quantity)
                {
                    throw CustomException.BadRequest(
                        $"Artwork {artwork.Title} does not have enough stock. Requested: {orderDetail.Quantity}, Available: {artwork.Quantity}."
                    );
                }

                // Reduce artwork stock
                artwork.Quantity -= orderDetail.Quantity;

                // Update the artwork quantity in the repository
                await _artworkRepository.UpdateOneAsync(artwork);

                // Calculate the total amount for this order detail
                decimal detailAmount = artwork.Price * orderDetail.Quantity;

                // Add this amount to the total amount
                totalAmount += detailAmount;
            }

            var newOrder = _mapper.Map<OrderCreateDto, Order>(createDto);

            // Set the user ID on the new order
            newOrder.UserId = userId;
            newOrder.TotalAmount = totalAmount;
            newOrder.Status = OrderStatus.InProgress;

            // Save the order to the repository
            var createdOrder = await _orderRepository.CreateOneAsync(newOrder);

            // Return the created order as a DTO
            return _mapper.Map<Order, OrderReadDto>(createdOrder);
        }

        public async Task<OrderReadDto> UpdateStatusToShippedAsync(Guid id)
        {
            var foundOrder = await _orderRepository.GetByIdAsync(id);
            if (foundOrder == null)
            {
                throw CustomException.NotFound($"Order with ID {id} not found.");
            }
            if (foundOrder.Status.ToString() != OrderStatus.InProgress.ToString())
            {
                throw CustomException.BadRequest($"Invalid updating Status");
            }
            foundOrder.Status = OrderStatus.Shipped;
            var updatedOrder = await _orderRepository.UpdateOneAsync(foundOrder);
            return _mapper.Map<Order, OrderReadDto>(updatedOrder);
        }

        public async Task<OrderReadDto> UpdateStatusToDeliveredAsync(Guid id)
        {
            var foundOrder = await _orderRepository.GetByIdAsync(id);
            if (foundOrder == null)
            {
                throw CustomException.NotFound($"Order with ID {id} not found.");
            }
            if (foundOrder.Status.ToString() != OrderStatus.Shipped.ToString())
            {
                throw CustomException.BadRequest($"Invalid updating Status");
            }

            foundOrder.Status = OrderStatus.Delivered;
            var updatedOrder = await _orderRepository.UpdateOneAsync(foundOrder);
            return _mapper.Map<Order, OrderReadDto>(updatedOrder);
        }

        public async Task<OrderReadDto> UpdateStatusToCanceledAsync(Guid id)
        {
            var foundOrder = await _orderRepository.GetByIdAsync(id);
            if (foundOrder == null)
            {
                throw CustomException.NotFound($"Order with ID {id} not found.");
            }

            if (foundOrder.Status.ToString() != OrderStatus.InProgress.ToString())
            {
                throw CustomException.BadRequest($"Invalid updating Status");
            }

            foundOrder.Status = OrderStatus.Canceled;
            var updatedOrder = await _orderRepository.UpdateOneAsync(foundOrder);
            return _mapper.Map<Order, OrderReadDto>(updatedOrder);
        }
    }
}
