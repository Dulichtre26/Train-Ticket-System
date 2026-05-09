using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TrainTicket.Data.Entities;
using Route = TrainTicket.Data.Entities.Route;
using BCrypt.Net;

namespace TrainTicket.Data.DbContexts
{
    public static class DataSeeder
    {
        public static void Initialize(TrainTicketDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Stations.Any()) return; 

            var stationHanoi = new Station { StationCode = "HAN", StationName = "Hà N?i", City = "Hà N?i", IsActive = true, RegionCode = "HQ" };
            var stationSG = new Station { StationCode = "SGN", StationName = "Sài Gòn", City = "TP.HCM", IsActive = true, RegionCode = "HQ" };
            var stationDN = new Station { StationCode = "DAD", StationName = "?à N?ng", City = "?à N?ng", IsActive = true, RegionCode = "HQ" };

            context.Stations.AddRange(stationHanoi, stationSG, stationDN);

            var train1 = new Train { TrainCode = "SE1", TrainName = "Tàu Khách B?c Nam SE1", TrainType = "Nhanh", IsActive = true, RegionCode = "HQ" };
            var train2 = new Train { TrainCode = "SE2", TrainName = "Tàu Khách Nam B?c SE2", TrainType = "Nhanh", IsActive = true, RegionCode = "HQ" };

            context.Trains.AddRange(train1, train2);
            context.SaveChanges();

            var route1 = new Route { RouteName = "Hà N?i - Sài Gòn", DepartureStation = stationHanoi.StationID, ArrivalStation = stationSG.StationID, Distance = 1726, RegionCode = "HQ" };
            var route2 = new Route { RouteName = "Sài Gòn - Hà N?i", DepartureStation = stationSG.StationID, ArrivalStation = stationHanoi.StationID, Distance = 1726, RegionCode = "HQ" };
            
            context.Routes.AddRange(route1, route2);
            context.SaveChanges();

            var carriage1 = new Carriage { TrainID = train1.TrainID, CarriageCode = "C1", CarriageType = "VIP", TotalSeats = 2, IsActive = true, RegionCode = "HQ" };
            context.Carriages.Add(carriage1);
            context.SaveChanges();

            var seat1 = new Seat { CarriageID = carriage1.CarriageID, SeatNumber = "1A", SeatType = "Ng?i M?m", IsActive = true, RegionCode = "HQ" };
            var seat2 = new Seat { CarriageID = carriage1.CarriageID, SeatNumber = "1B", SeatType = "Ng?i M?m", IsActive = true, RegionCode = "HQ" };
            context.Seats.AddRange(seat1, seat2);
            context.SaveChanges();

            var schedule1 = new Schedule { TrainID = train1.TrainID, RouteID = route1.RouteID, DepartureTime = DateTime.Now.AddDays(1), ArrivalTime = DateTime.Now.AddDays(2), Status = "Scheduled", RegionCode = "HQ" };
            context.Schedules.Add(schedule1);
            context.SaveChanges();

            var schedulePrice = new SchedulePrice { ScheduleID = schedule1.ScheduleID, SeatType = "Ng?i M?m", Price = 1200000, RegionCode = "HQ" };
            context.SchedulePrices.Add(schedulePrice);
            context.SaveChanges();
            
            var roleAdmin = new Role { RoleName = "Admin", Description = "Administrator" };
            var roleUser = new Role { RoleName = "User", Description = "Customer" };
            context.Roles.AddRange(roleAdmin, roleUser);
            context.SaveChanges();

            var adminUser = new User { Email = "admin@trainticket.vn", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"), FullName = "Admin", PhoneNumber = "0123456789", IsActive = true, RegionCode = "HQ" };
            var customerUser = new User { Email = "kh@trainticket.vn", PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"), FullName = "Khách Hàng 1", PhoneNumber = "0987654321", IsActive = true, RegionCode = "HQ" };
            context.Users.AddRange(adminUser, customerUser);
            context.SaveChanges();

            context.UserRoles.Add(new UserRole { UserID = adminUser.UserID, RoleID = roleAdmin.RoleID });
            context.UserRoles.Add(new UserRole { UserID = customerUser.UserID, RoleID = roleUser.RoleID });
            context.SaveChanges();

            var ticket = new Ticket { TicketCode = "TCK123456", UserID = customerUser.UserID, ScheduleID = schedule1.ScheduleID, SeatID = seat1.SeatID, PassengerName = "Khách Hàng 1", PassengerID = "123456789", PassengerPhone = "0987654321", SeatType = "Ng?i M?m", OriginalPrice = 1200000, FinalPrice = 1200000, Status = "Paid", BookedAt = DateTime.Now, RegionCode = "HQ" };
            context.Tickets.Add(ticket);
            context.SaveChanges();

            var payment = new Payment { TicketID = ticket.TicketID, PaymentMethod = "Cash", Amount = 1200000, PaidAt = DateTime.Now, Status = "Completed" };
            context.Payments.Add(payment);
            context.SaveChanges();
        }
    }
}