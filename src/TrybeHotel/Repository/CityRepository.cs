using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class CityRepository : ICityRepository
    {
        protected readonly ITrybeHotelContext _context;
        public CityRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        // 4. Refatore o endpoint GET /city
        public IEnumerable<CityDto> GetCities()
        {
            return _context.Cities.Select(city => new CityDto
            {
                cityId = city.CityId,
                name = city.Name,
                state = city.State,
            }).ToList();
        }

        // 2. Refatore o endpoint POST /city
        public CityDto AddCity(City city)
        {
            _context.Cities.Add(city);
            _context.SaveChanges();

            return _context.Cities.Select(city => new CityDto
            {
                cityId = city.CityId,
                name = city.Name,
                state = city.State,
            }).Last();
        }

        // 3. Desenvolva o endpoint PUT /city
        public CityDto UpdateCity(City city)
        {
            City cityUpdated = _context.Cities.FirstOrDefault(c => c.CityId == city.CityId)!;
            
            cityUpdated.Name = city.Name;
      	    cityUpdated.State = city.State;
      	    
      	    _context.SaveChanges();

      	    return new CityDto
      	    {
        	cityId = cityUpdated.CityId,
        	name = cityUpdated.Name,
        	state = cityUpdated.State,
      	    };
        }

    }
}
