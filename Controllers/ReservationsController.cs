using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HotelMng.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.ML;
using Newtonsoft.Json;

namespace HotelMng.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservationsController : ControllerBase
    {

        private readonly ReservationContext _context;


        public ReservationsController(ReservationContext context)
        {
            _context = context;

        }



        // GET: api/reservations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations()
        {

            return await _context.Reservations.ToListAsync();
        }




        // GET: api/reservation/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reservation>> GetReservation(long id)
        {
            var reservation = await _context.Reservations.FindAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }
            return reservation;
        }




        // PUT: api/reservations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReservation(long id, Reservation reservation)
        {
            if (id != reservation.id)
            {
                return BadRequest();
            }

            if (!ReservationExists(id))
            {
                return NotFound();
            }


            // validator
            String[] dateFormats = { "dd.MM.yyyy", "dd-MM-yyyy", "dd/MM/yyyy" };
            if (!IsValidDate(reservation.arrivalDate, dateFormats) || !IsValidDate(reservation.departureDate, dateFormats))
            {
                throw new ArgumentException("Date format not valid");
            }

            if (string.IsNullOrEmpty(reservation.guest))
            {
                throw new ArgumentException("Guest name must not be null");
            }

            if (reservation.noOfPersons == 0)
            {
                throw new ArgumentException("Number of persons must be greater than 0");
            }

            if (reservation.roomFare == 0)
            {
                throw new ArgumentException("Room fare must be greater than 0");
            }


            _context.Entry(reservation).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(reservation);
        }






        // POST: api/reservation
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<IActionResult> PostReservation(Reservation reservation)
        {

            // validator
            String[] dateFormats = { "dd.MM.yyyy", "dd-MM-yyyy", "dd/MM/yyyy" };
            if (!IsValidDate(reservation.arrivalDate, dateFormats) || !IsValidDate(reservation.departureDate, dateFormats))
            {
                // return HttpResponseException("Date format not valid");
                throw new ArgumentException("Date format not valid");
            }

            if (string.IsNullOrEmpty(reservation.guest))
            {
                //   return HttpResponseException("Guest name must not be null");
                throw new ArgumentException("Guest name must not be null");
            }

            if (reservation.noOfPersons == 0)
            {
                // return HttpResponseException("Number of persons must be greater than 0");
                throw new ArgumentException("Number of persons must be greater than 0");
            }

            if (reservation.roomFare == 0)
            {
                throw new ArgumentException("Room fare must be greater than 0");
            }



            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();


            // return CreatedAtAction("GetReservation", new { id = reservation.id }, reservation);
            return Ok(reservation);
        }






        // DELETE: api//5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Reservation>> DeleteReservation(long id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            _context.Reservations.Remove(reservation);
            //_context.Entry(reminderItem).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            return reservation;
        }



        private bool ReservationExists(long id)
        {
            return _context.Reservations.Any(e => e.id == id);
        }




        // validate Date format
        public static bool IsValidDate(string value, string[] dateFormats)
        {
            DateTime tempDate;
            bool validDate = DateTime.TryParseExact(value, dateFormats, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out tempDate);
            if (validDate)
                return true;
            else
                return false;
        }






        

    }
}

