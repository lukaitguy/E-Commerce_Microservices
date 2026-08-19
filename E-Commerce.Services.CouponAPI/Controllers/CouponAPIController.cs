using AutoMapper;
using E_Commerce.Services.CouponAPI.Data;
using E_Commerce.Services.CouponAPI.Models;
using E_Commerce.Services.CouponAPI.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Services.CouponAPI.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResultDto _res;
        private IMapper _mapper;

        public CouponAPIController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _res = new ResultDto();
            _mapper = mapper;
        }

        [HttpGet]
        public ResultDto GetAll()
        {
            try
            {
                IEnumerable<Coupon> objList = _db.Coupons.ToList();
                _res.Result = _mapper.Map<IEnumerable<CouponDTO>>(objList);
            }
            catch(Exception ex)
            {
                _res.Success = false;
                _res.Message = ex.Message;
            }
            return _res;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResultDto GetById(int id)
        {
            try
            {
                Coupon cp = _db.Coupons.First(c => c.CouponId == id);    
                _res.Result = _mapper.Map<CouponDTO>(cp);
            }
            catch(Exception ex)
            {
                _res.Success = false;
                _res.Message = ex.Message;
            }
            return _res;
        }

        [HttpGet]
        [Route("GetByCode/{code}")]
        public ResultDto GetByCode(string code)
        {
            try
            {
                Coupon cp = _db.Coupons.First(c => c.CouponCode.ToLower() == code.ToLower());
                _res.Result = _mapper.Map<CouponDTO>(cp);
            }
            catch (Exception ex)
            {
                _res.Success = false;
                _res.Message = ex.Message;
            }
            return _res;
        }

        [HttpPost]
        public ResultDto Create([FromBody] CouponDTO couponDto)
        {
            try
            {
                Coupon cp = _mapper.Map<Coupon>(couponDto);
                _db.Coupons.Add(cp);
                _db.SaveChanges();
                _res.Result = _mapper.Map<CouponDTO>(cp);
            }
            catch (Exception ex)
            {
                _res.Success = false;
                _res.Message = ex.Message;
            }
            return _res;
        }

        [HttpPut]
        public ResultDto Update([FromBody] CouponDTO couponDto)
        {
            try
            {
                Coupon cp = _mapper.Map<Coupon>(couponDto);
                _db.Coupons.Update(cp);
                _db.SaveChanges();
                _res.Result = _mapper.Map<CouponDTO>(cp);
            }
            catch (Exception ex)
            {
                _res.Success = false;
                _res.Message = ex.Message;
            }
            return _res;
        }

        [HttpDelete]
        [Route("{id:int}")]
        public ResultDto Delete(int id)
        {
            try
            {
                Coupon cp = _db.Coupons.First(c => c.CouponId == id);
                _db.Coupons.Remove(cp);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _res.Success = false;
                _res.Message = ex.Message;
            }
            return _res;
        }
    }
}
