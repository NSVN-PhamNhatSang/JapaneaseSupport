using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JLearning.Repository;
using JLearning.Models;
using Microsoft.EntityFrameworkCore;
using JLearning.Data;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using JLearning.Interfaces;
using JLearning.Dto;
using AutoMapper;

namespace JLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDetailController : ControllerBase
    {
        private readonly IUserDetailRepository _userDetailRepository;
        private readonly IMapper _mapper;


        public UserDetailController(IUserDetailRepository userDetailRepository,IMapper mapper)
        {
            _userDetailRepository = userDetailRepository;
            _mapper = mapper;   
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserDetail>))]
        public async Task<IActionResult> GetAll()
        {
            var userDetails = _mapper.Map<List<UserDetail>>(_userDetailRepository.GetAll());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            } else
                return Ok(userDetails);


        }
        [HttpGet("{userId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserDetail>))]
        [ProducesResponseType(400)]
         public IActionResult  getUserById(string userId) { 
        var userDetail = _userDetailRepository.GetUserById(userId) ;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
                return Ok(userDetail);
        }


        [HttpPut("{userId}")]
        [ProducesResponseType(200, Type = typeof(UserDetail))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateUserDetail(string userId, [FromBody] UserDetail userdetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedUserDetail = _userDetailRepository.UpdateInfo(userId, userdetail);
                return Ok(updatedUserDetail);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi khác (nếu có)
                return StatusCode(500, "Internal server error");
            }
        }
    }


}

