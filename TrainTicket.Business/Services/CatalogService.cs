using Microsoft.EntityFrameworkCore;
using TrainTicket.Business.DTOs;
using TrainTicket.Business.Interfaces;
using TrainTicket.Data.DbContexts;
using TrainTicket.Data.Entities;

namespace TrainTicket.Business.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly TrainTicketDbContext _db;

        public CatalogService(TrainTicketDbContext db)
        {
            _db = db;
        }

        // ?? TRAINS ???????????????????????????????????????
        public async Task<List<TrainDto>> GetAllTrainsAsync()
        {
            var trains = await _db.Trains
                .Where(t => t.IsActive == true) // Gi? ??nh ch? l?y các tàu ?ang ho?t ??ng/ch?a b? soft delete
                .OrderBy(t => t.TrainCode)
                .ToListAsync();

            return trains.Select(t => new TrainDto
            {
                TrainId = t.TrainId,
                TrainCode = t.TrainCode,
                TrainName = t.TrainName,
                TrainType = t.TrainType,
                IsActive = t.IsActive ?? false,
                CreatedAt = t.CreatedAt ?? DateTime.Now
            }).ToList();
        }

        public async Task<TrainDto?> GetTrainByIdAsync(int id)
        {
            var t = await _db.Trains.FindAsync(id);
            if (t == null) return null;

            return new TrainDto
            {
                TrainId = t.TrainId,
                TrainCode = t.TrainCode,
                TrainName = t.TrainName,
                TrainType = t.TrainType,
                IsActive = t.IsActive ?? false,
                CreatedAt = t.CreatedAt ?? DateTime.Now
            };
        }

        public async Task<bool> SaveTrainAsync(TrainDto train)
        {
            if (train.TrainId == 0)
            {
                var newTrain = new Train
                {
                    TrainCode = train.TrainCode,
                    TrainName = train.TrainName,
                    TrainType = train.TrainType,
                    CreatedAt = DateTime.Now,
                    IsActive = true
                };
                await _db.Trains.AddAsync(newTrain);
            }
            else
            {
                var existing = await _db.Trains.FindAsync(train.TrainId);
                if (existing == null) return false;

                existing.TrainCode = train.TrainCode;
                existing.TrainName = train.TrainName;
                existing.TrainType = train.TrainType;
            }
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTrainAsync(int id)
        {
            var t = await _db.Trains.FindAsync(id);
            if (t == null) return false;
            
            // Soft delete
            t.IsActive = false;
            await _db.SaveChangesAsync();
            return true;
        }

        // ?? STATIONS ?????????????????????????????????????
        public async Task<List<StationDto>> GetAllStationsAsync()
        {
            var stations = await _db.Stations
                .Where(s => s.IsActive == true)
                .OrderBy(s => s.City)
                .ThenBy(s => s.StationName)
                .ToListAsync();

            return stations.Select(s => new StationDto
            {
                StationId = s.StationId,
                StationCode = s.StationCode,
                StationName = s.StationName,
                City = s.City,
                Address = s.Address,
                IsActive = s.IsActive ?? false,
                CreatedAt = s.CreatedAt ?? DateTime.Now
            }).ToList();
        }

        public async Task<StationDto?> GetStationByIdAsync(int id)
        {
            var s = await _db.Stations.FindAsync(id);
            if (s == null) return null;

            return new StationDto
            {
                StationId = s.StationId,
                StationCode = s.StationCode,
                StationName = s.StationName,
                City = s.City,
                Address = s.Address,
                IsActive = s.IsActive ?? false,
                CreatedAt = s.CreatedAt ?? DateTime.Now
            };
        }

        public async Task<bool> SaveStationAsync(StationDto station)
        {
            if (station.StationId == 0)
            {
                var newStation = new Station
                {
                    StationCode = station.StationCode,
                    StationName = station.StationName,
                    City = station.City,
                    Address = station.Address,
                    CreatedAt = DateTime.Now,
                    IsActive = true
                };
                await _db.Stations.AddAsync(newStation);
            }
            else
            {
                var existing = await _db.Stations.FindAsync(station.StationId);
                if (existing == null) return false;

                existing.StationCode = station.StationCode;
                existing.StationName = station.StationName;
                existing.City = station.City;
                existing.Address = station.Address;
                _db.Stations.Update(existing);
            }
            
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteStationAsync(int id)
        {
            var s = await _db.Stations.FindAsync(id);
            if (s == null) return false;
            
            s.IsActive = false;
            await _db.SaveChangesAsync();
            return true;
        }

        // ?? ROUTES ???????????????????????????????????????
        public async Task<List<RouteDto>> GetAllRoutesAsync()
        {
            var routes = await _db.Routes
                .Include(r => r.DepartureStationNavigation)
                .Include(r => r.ArrivalStationNavigation)
                .Where(r => r.IsActive == true)
                .OrderBy(r => r.RouteName)
                .ToListAsync();

            return routes.Select(r => new RouteDto
            {
                RouteId = r.RouteId,
                RouteName = r.RouteName,
                DepartureStation = r.DepartureStation,
                ArrivalStation = r.ArrivalStation,
                Distance = r.Distance,
                IsActive = r.IsActive ?? false,
                CreatedAt = r.CreatedAt ?? DateTime.Now,
                DepartureStationName = r.DepartureStationNavigation?.StationName,
                ArrivalStationName = r.ArrivalStationNavigation?.StationName
            }).ToList();
        }

        public async Task<RouteDto?> GetRouteByIdAsync(int id)
        {
            var r = await _db.Routes
                .Include(rt => rt.DepartureStationNavigation)
                .Include(rt => rt.ArrivalStationNavigation)
                .FirstOrDefaultAsync(rt => rt.RouteId == id);

            if (r == null) return null;

            return new RouteDto
            {
                RouteId = r.RouteId,
                RouteName = r.RouteName,
                DepartureStation = r.DepartureStation,
                ArrivalStation = r.ArrivalStation,
                Distance = r.Distance,
                IsActive = r.IsActive ?? false,
                CreatedAt = r.CreatedAt ?? DateTime.Now,
                DepartureStationName = r.DepartureStationNavigation?.StationName,
                ArrivalStationName = r.ArrivalStationNavigation?.StationName
            };
        }

        public async Task<bool> SaveRouteAsync(RouteDto route)
        {
            if (route.RouteId == 0)
            {
                var newRoute = new Data.Entities.Route
                {
                    RouteName = route.RouteName,
                    DepartureStation = route.DepartureStation,
                    ArrivalStation = route.ArrivalStation,
                    Distance = route.Distance,
                    CreatedAt = DateTime.Now,
                    IsActive = true
                };
                await _db.Routes.AddAsync(newRoute);
            }
            else
            {
                var existing = await _db.Routes.FindAsync(route.RouteId);
                if (existing == null) return false;

                existing.RouteName = route.RouteName;
                existing.DepartureStation = route.DepartureStation;
                existing.ArrivalStation = route.ArrivalStation;
                existing.Distance = route.Distance;
                _db.Routes.Update(existing);
            }
            
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRouteAsync(int id)
        {
            var r = await _db.Routes.FindAsync(id);
            if (r == null) return false;
            
            r.IsActive = false;
            await _db.SaveChangesAsync();
            return true;
        }

        // ?? SCHEDULES ????????????????????????????????????
        public async Task<List<ScheduleDto>> GetAllSchedulesAsync()
        {
            var schedules = await _db.Schedules
                .Include(s => s.Train)
                .Include(s => s.Route)
                    .ThenInclude(r => r.DepartureStationNavigation)
                .Include(s => s.Route)
                    .ThenInclude(r => r.ArrivalStationNavigation)
                .Where(s => s.IsActive == true)
                .OrderByDescending(s => s.DepartureTime)
                .ToListAsync();

            return schedules.Select(s => new ScheduleDto
            {
                ScheduleId = s.ScheduleId,
                TrainId = s.TrainId,
                RouteId = s.RouteId,
                DepartureTime = s.DepartureTime,
                ArrivalTime = s.ArrivalTime,
                Status = s.Status,
                IsActive = s.IsActive ?? false,
                CreatedAt = s.CreatedAt ?? DateTime.Now,
                TrainName = s.Train?.TrainName,
                RouteName = s.Route?.RouteName
            }).ToList();
        }

        public async Task<ScheduleDto?> GetScheduleByIdAsync(int id)
        {
            var s = await _db.Schedules
                .Include(sch => sch.Train)
                .Include(sch => sch.Route)
                    .ThenInclude(r => r.DepartureStationNavigation)
                .Include(sch => sch.Route)
                    .ThenInclude(r => r.ArrivalStationNavigation)
                .FirstOrDefaultAsync(sch => sch.ScheduleId == id);

            if (s == null) return null;

            return new ScheduleDto
            {
                ScheduleId = s.ScheduleId,
                TrainId = s.TrainId,
                RouteId = s.RouteId,
                DepartureTime = s.DepartureTime,
                ArrivalTime = s.ArrivalTime,
                Status = s.Status,
                IsActive = s.IsActive ?? false,
                CreatedAt = s.CreatedAt ?? DateTime.Now,
                TrainName = s.Train?.TrainName,
                RouteName = s.Route?.RouteName
            };
        }

        public async Task<bool> SaveScheduleAsync(ScheduleDto schedule)
        {
            if (schedule.ScheduleId == 0)
            {
                var newSchedule = new Schedule
                {
                    TrainId = schedule.TrainId,
                    RouteId = schedule.RouteId,
                    DepartureTime = schedule.DepartureTime,
                    ArrivalTime = schedule.ArrivalTime,
                    Status = schedule.Status,
                    CreatedAt = DateTime.Now,
                    IsActive = true
                };
                await _db.Schedules.AddAsync(newSchedule);
            }
            else
            {
                var existing = await _db.Schedules.FindAsync(schedule.ScheduleId);
                if (existing == null) return false;

                existing.TrainId = schedule.TrainId;
                existing.RouteId = schedule.RouteId;
                existing.DepartureTime = schedule.DepartureTime;
                existing.ArrivalTime = schedule.ArrivalTime;
                existing.Status = schedule.Status;
                _db.Schedules.Update(existing);
            }
            
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteScheduleAsync(int id)
        {
            var s = await _db.Schedules.FindAsync(id);
            if (s == null) return false;
            
            s.IsActive = false;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
