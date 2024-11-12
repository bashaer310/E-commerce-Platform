using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Backend_Teamwork.src.Entities;
using Backend_Teamwork.src.Services.booking;
using Backend_Teamwork.src.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Backend_Teamwork.src.DTO.BookingDTO;

namespace Backend_Teamwork.src.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<BookingReadDto>>> GetBookings(
            [FromQuery] PaginationOptions paginationOptions
        )
        {
            var bookingList = await _bookingService.GetAllAsync(paginationOptions);
            var totalCount = await _bookingService.GetCountAsync();
            var bookingResponse = new { BookingList = bookingList, TotalCount = totalCount };
            return Ok(bookingResponse);
        }

        [HttpGet("customer")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<List<BookingReadDto>>> GetAllBookingsByUserId(
            [FromQuery] PaginationOptions paginationOptions
        )
        {
            var authClaims = HttpContext.User;
            var userId = authClaims.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
            var userGuid = new Guid(userId);

            var bookingList = await _bookingService.GetAllByUserIdAsync(
                paginationOptions,
                userGuid
            );
            var totalCount = await _bookingService.GetCountByUserIdAsync(userGuid);
            var bookingResponse = new { BookingList = bookingList, TotalCount = totalCount };
            return Ok(bookingResponse);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<ActionResult<BookingReadDto>> GetBookingById([FromRoute] Guid id)
        {
            var authClaims = HttpContext.User;
            var userId = authClaims.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
            var userRole = authClaims.FindFirst(c => c.Type == ClaimTypes.Role)!.Value;
            var convertedUserId = new Guid(userId);
            var booking = await _bookingService.GetByIdAsync(id, convertedUserId, userRole);
            return Ok(booking);
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<BookingReadDto>> CreateBooking(
            [FromBody] BookingCreateDto bookingDTO
        )
        {
            var authClaims = HttpContext.User;
            var userId = authClaims.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
            var convertedUserId = new Guid(userId);
            var booking = await _bookingService.CreateAsync(bookingDTO, convertedUserId);
            return CreatedAtAction(nameof(CreateBooking), new { id = booking.Id }, booking);
        }

        [HttpPut("confirming/{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BookingReadDto>> UpdateBookingStatusToConfirmed(
            [FromRoute] Guid id
        )
        {
            var booking = await _bookingService.UpdateStatusToConfirmedAsync(id);
            return Ok(booking);
        }

        [HttpPut("canceling/{id:guid}")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<BookingReadDto>> UpdateBookingStatusToCanceled(
            [FromRoute] Guid id
        )
        {
            var authClaims = HttpContext.User;
            var userId = authClaims.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
            var convertedUserId = new Guid(userId);
            var booking = await _bookingService.UpdateStatusToCanceledAsync(id, convertedUserId);
            return Ok(booking);
        }
    }
}
