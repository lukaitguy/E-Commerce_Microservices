using AutoMapper;
using E_Commerce.Services.ProductAPI.Data;
using E_Commerce.Services.ProductAPI.Models;
using E_Commerce.Services.ProductAPI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Services.ProductAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    [Authorize]
    public class ProductAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResultDto _res;
        private IMapper _mapper;

        public ProductAPIController(AppDbContext db, IMapper mapper)
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
                IEnumerable<Product> objList = _db.Products.ToList();
                _res.Result = _mapper.Map<IEnumerable<ProductDTO>>(objList);
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
                Product pr = _db.Products.First(c => c.ProductId == id);    
                _res.Result = _mapper.Map<ProductDTO>(pr);
            }
            catch(Exception ex)
            {
                _res.Success = false;
                _res.Message = ex.Message;
            }
            return _res;
        }


        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public ResultDto Create([FromBody] ProductDTO productDto)
        {
            try
            {
                Product pr = _mapper.Map<Product>(productDto);
                _db.Products.Add(pr);
                _db.SaveChanges();
                _res.Result = _mapper.Map<ProductDTO>(pr);
            }
            catch (Exception ex)
            {
                _res.Success = false;
                _res.Message = ex.Message;
            }
            return _res;
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public ResultDto Update([FromBody] ProductDTO productDto)
        {
            try
            {
                Product pr = _mapper.Map<Product>(productDto);
                _db.Products.Update(pr);
                _db.SaveChanges();
                _res.Result = _mapper.Map<ProductDTO>(pr);
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
        [Authorize(Roles = "ADMIN")]
        public ResultDto Delete(int id)
        {
            try
            {
                Product pr = _db.Products.First(c => c.ProductId == id);
                _db.Products.Remove(pr);
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
