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
                .Where(t => t.IsActive) // Gi? ??nh ch? l?y các tàu ?ang ho?t ??ng/ch?a b? soft delete
                .OrderBy(t => t.TrainCode)
                .ToListAsync();

            return trains.Select(t => new TrainDto
            {
                TrainID = t.TrainID,
                TrainCode = t.TrainCode,
                TrainName = t.TrainName,
                TrainType = t.TrainType,
                IsActive = t.IsActive,
                CreatedAt = t.CreatedAt,
                RegionCode = t.RegionCode
            }).ToList();
        }

        public async Task<TrainDto?> GetTrainByIdAsync(int id)
        {
            var t = await _db.Trains.FindAsync(id);
            if (t == null) return null;

            return new TrainDto
            {
                TrainID = t.TrainID,
                TrainCode = t.TrainCode,
                TrainName = t.TrainName,
                TrainType = t.TrainType,
                IsActive = t.IsActive,
                CreatedAt = t.CreatedAt,
                RegionCode = t.RegionCode
            };
        }

        public async Task<bool> SaveTrainAsync(TrainDto train)
        {
            if (train.TrainID == 0)
            {
                var newTrain = new Train
                {
                    TrainCode = train.TrainCode,
                    TrainName = train.TrainName,
                    TrainType = train.TrainType,
                    CreatedAt = DateTime.Now,
                    IsActive = true,
                    RegionCode = train.RegionCode
                };
                await _db.Trains.AddAsync(newTrain);
            }
            else
            {
                var existing = await _db.Trains.FindAsync(train.TrainID);
                if (existing == null) return false;

                existing.TrainCode = train.TrainCode;
                existing.TrainName = train.TrainName;
                existing.TrainType = train.TrainType;
                _db.Trains.Update(existing);
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
                .Where(s => s.IsActive)
                .OrderBy(s => s.City)
                .ThenBy(s => s.StationName)
                .ToListAsync();

            return stations.Select(s => new StationDto
            {
                StationID = s.StationID,
                StationCode = s.StationCode,
                StationName = s.StationName,
                City = s.City,
                Address = s.Address,
                IsActive = s.IsActive,
                CreatedAt = s.CreatedAt,
                RegionCode = s.RegionCode
            }).ToList();
        }

        public async Task<StationDto?> GetStationByIdAsync(int id)
        {
            var s = await _db.Stations.FindAsync(id);
            if (s == null) return null;

            return new StationDto
            {
                StationID = s.StationID,
                StationCode = s.StationCode,
                StationName = s.StationName,
                City = s.City,
                Address = s.Address,
                IsActive = s.IsActive,
                CreatedAt = s.CreatedAt,
                RegionCode = s.RegionCode
            };
        }

        public async Task<bool> SaveStationAsync(StationDto station)
        {
            if (station.StationID == 0)
            {
                var newStation = new Station
                {
                    StationCode = station.StationCode,
                    StationName = station.StationName,
                    City = station.City,
                    Address = station.Address,
                    CreatedAt = DateTime.Now,
                    IsActive = true,
                    RegionCode = station.RegionCode
                };
                await _db.Stations.AddAsync(newStation);
            }
            else
            {
                var existing = await _db.Stations.FindAsync(station.StationID);
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
                .Include(r => r.DepartureStationNav)
                .Include(r => r.ArrivalStationNav)
                .Where(r => r.IsActive)
                .OrderBy(r => r.RouteName)
                .ToListAsync();

            return routes.Select(r => new RouteDto
            {
                RouteID = r.RouteID,
                RouteName = r.RouteName,
                DepartureStation = r.DepartureStation,
                ArrivalStation = r.ArrivalStation,
                Distance = r.Distance,
                RouteType = r.RouteType,
                IsActive = r.IsActive,
                CreatedAt = r.CreatedAt,
                RegionCode = r.RegionCode,
                DepartureStationName = r.DepartureStationNav?.StationName,
                ArrivalStationName = r.ArrivalStationNav?.StationName
            }).ToList();
        }

        public async Task<RouteDto?> GetRouteByIdAsync(int id)
        {
            var r = await _db.Routes
                .Include(rt => rt.DepartureStationNav)
                .Include(rt => rt.ArrivalStationNav)
                .FirstOrDefaultAsync(rt => rt.RouteID == id);

            if (r == null) return null;

            return new RouteDto
            {
                RouteID = r.RouteID,
                RouteName = r.RouteName,
                DepartureStation = r.DepartureStation,
                ArrivalStation = r.ArrivalStation,
                Distance = r.Distance,
                RouteType = r.RouteType,
                IsActive = r.IsActive,
                CreatedAt = r.CreatedAt,
                RegionCode = r.RegionCode,
                DepartureStationName = r.DepartureStationNav?.StationName,
                ArrivalStationName = r.ArrivalStationNav?.StationName
            };
        }

        public async Task<bool> SaveRouteAsync(RouteDto route)
        {
            if (route.RouteID == 0)
            {
                var newRoute = new Data.Entities.Route
                {
                    RouteName = route.RouteName,
                    DepartureStation = route.DepartureStation,
                    ArrivalStation = route.ArrivalStation,
                    Distance = route.Distance,
                    RouteType = route.RouteType,
                    CreatedAt = DateTime.Now,
                    IsActive = true,
                    RegionCode = route.RegionCode
                };
                await _db.Routes.AddAsync(newRoute);
            }
            else
            {
                var existing = await _db.Routes.FindAsync(route.RouteID);
                if (existing == null) return false;

                existing.RouteName = route.RouteName;
                existing.DepartureStation = route.DepartureStation;
                existing.ArrivalStation = route.ArrivalStation;
                existing.Distance = route.Distance;
                existing.RouteType = route.RouteType;
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
                    .ThenInclude(r => r.DepartureStationNav)
                .Include(s => s.Route)
                    .ThenInclude(r => r.ArrivalStationNav)
                .Where(s => s.IsActive)
                .OrderByDescending(s => s.DepartureTime)
                .ToListAsync();

            return schedules.Select(s => new ScheduleDto
            {
                ScheduleID = s.ScheduleID,
                TrainID = s.TrainID,
                RouteID = s.RouteID,
                DepartureTime = s.DepartureTime,
                ArrivalTime = s.ArrivalTime,
                Status = s.Status,
                IsActive = s.IsActive,
                CreatedAt = s.CreatedAt,
                RegionCode = s.RegionCode,
                TrainName = s.Train?.TrainName,
                RouteName = s.Route?.RouteName
            }).ToList();
        }

        public async Task<ScheduleDto?> GetScheduleByIdAsync(int id)
        {
            var s = await _db.Schedules
                .Include(sch => sch.Train)
                .Include(sch => sch.Route)
                    .ThenInclude(r => r.DepartureStationNav)
                .Include(sch => sch.Route)
                    .ThenInclude(r => r.ArrivalStationNav)
                .FirstOrDefaultAsync(sch => sch.ScheduleID == id);

            if (s == null) return null;

            return new ScheduleDto
            {
                ScheduleID = s.ScheduleID,
                TrainID = s.TrainID,
                RouteID = s.RouteID,
                DepartureTime = s.DepartureTime,
                ArrivalTime = s.ArrivalTime,
                Status = s.Status,
                IsActive = s.IsActive,
                CreatedAt = s.CreatedAt,
                RegionCode = s.RegionCode,
                TrainName = s.Train?.TrainName,
                RouteName = s.Route?.RouteName
            };
        }

        public async Task<bool> SaveScheduleAsync(ScheduleDto schedule)
        {
            if (schedule.ScheduleID == 0)
            {
                var newSchedule = new Schedule
                {
                    TrainID = schedule.TrainID,
                    RouteID = schedule.RouteID,
                    DepartureTime = schedule.DepartureTime,
                    ArrivalTime = schedule.ArrivalTime,
                    Status = schedule.Status,
                    CreatedAt = DateTime.Now,
                    IsActive = true,
                    RegionCode = schedule.RegionCode
                };
                await _db.Schedules.AddAsync(newSchedule);
            }
            else
            {
                var existing = await _db.Schedules.FindAsync(schedule.ScheduleID);
                if (existing == null) return false;

                existing.TrainID = schedule.TrainID;
                existing.RouteID = schedule.RouteID;
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
