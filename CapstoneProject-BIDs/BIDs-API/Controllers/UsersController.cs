﻿using Microsoft.AspNetCore.Mvc;
using Data_Access.Entities;
using Business_Logic.Modules.UserModule.Interface;
using Business_Logic.Modules.UserModule.Request;
using AutoMapper;
using Business_Logic.Modules.UserModule.Response;
using Microsoft.AspNetCore.SignalR;
using BIDs_API.SignalR;

namespace BIDs_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public readonly IMapper _mapper;
        private readonly IHubContext<UserHub> _hubContext;

        public UsersController(IUserService userService
            , IMapper mapper
            , IHubContext<UserHub> hubContext)
        {
            _userService = userService;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        // GET api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseStaff>>> GetUsersForAdmin()
        {
            try
            {
                var list = await _userService.GetAll();
                var response = list.Select
                           (
                             emp => _mapper.Map<User, UserResponseStaff>(emp)
                           );
                if (response == null)
                {
                    return NotFound();
                }
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>
        [HttpGet("get-active")]
        public async Task<ActionResult<IEnumerable<UserResponseStaff>>> GetUsersActive()
        {
            try
            {
                var list = await _userService.GetUsersIsActive();
                var response = list.Select
                           (
                             emp => _mapper.Map<User, UserResponseStaff>(emp)
                           );
                if (response == null)
                {
                    return NotFound();
                }
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>
        [HttpGet("get-waitting")]
        public async Task<ActionResult<IEnumerable<UserResponseStaff>>> GetUsersWaitting()
        {
            try
            {
                var list = await _userService.GetUsersIsWaitting();
                var response = list.Select
                           (
                             emp => _mapper.Map<User, UserResponseStaff>(emp)
                           );
                if (response == null)
                {
                    return NotFound();
                }
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>
        [HttpGet("get-ban")]
        public async Task<ActionResult<IEnumerable<UserResponseStaff>>> GetUsersBan()
        {
            try
            {
                var list = await _userService.GetUsersIsBan();
                var response = list.Select
                           (
                             emp => _mapper.Map<User, UserResponseStaff>(emp)
                           );
                if (response == null)
                {
                    return NotFound();
                }
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> GetUserByID([FromRoute] Guid id)
        {
            var user = _mapper.Map<UserResponse>(await _userService.GetUserByID(id));

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_name/{name}")]
        public async Task<ActionResult<UserResponse>> GetUserByName([FromRoute] string name)
        {
            var user = _mapper.Map<UserResponse>(await _userService.GetUserByName(name));

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        //// GET api/<ValuesController>/abc
        //[HttpGet("by_account_name/{name}")]
        //public async Task<ActionResult<UserResponseStaff>> GetUserByAccountName([FromRoute] string name)
        //{
        //    var user = _mapper.Map<UserResponseStaff>(await _userService.GetUserByAccountName(name));

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return user;
        //}

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutUser([FromBody] UpdateUserRequest updateUserRequest)
        {
            try
            {
                var user = await _userService.UpdateUser(updateUserRequest);
                await _hubContext.Clients.All.SendAsync("ReceiveUserUpdate", user);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<ValuesController>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserResponse>> PostUser([FromBody] CreateUserRequest createUserRequest)
        {
            try
            {
                var user = await _userService.AddNewUser(createUserRequest);
                await _hubContext.Clients.All.SendAsync("ReceiveUserAdd", user);
                return Ok(_mapper.Map<UserResponse>(user));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //// DELETE api/<ValuesController>/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        //{
        //    try
        //    {
        //        await _userService.DeleteUser(id);
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        //private bool UserExists(Guid id)
        //{
        //    return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        //}
    }
}
