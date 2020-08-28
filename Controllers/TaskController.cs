using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lista_tarefas_api.Data;
using lista_tarefas_api.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace lista_tarefas_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly IRepository _repo;

        public TaskController(IRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskModel model)
        {
            try
            {
                if (model.When <= DateTime.UtcNow)
                {
                    return BadRequest("A tarefa à adicionar deve ser posterior ao horário atual");
                }

                model.Created = DateTime.UtcNow;

                _repo.Add(model);

                if (await _repo.SaveChangesAsync())
                {
                    return Ok(model);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex.Message}");
            }

            return BadRequest("Erro não esperado!!!");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> TaskById(string id)
        {
            try
            {
                var result = await _repo.GetTaskById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, TaskModel model)
        {
            try
            {
                var task = await _repo.GetTaskById(id);

                if (task == null)
                {
                    return NotFound();
                }

                if (model.When < DateTime.UtcNow)
                {
                    return BadRequest("A tarefa à adicionar deve ser posterior ao horário atual");
                }

                model.Id = task.Id;
                model.Created = DateTime.UtcNow;

                _repo.Update(model);

                if (await _repo.SaveChangesAsync())
                {
                    return Ok(model);
                }
                return BadRequest("Erro não esperado!!!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex.Message}");
            }
        }

        [HttpGet("filter/all/{macAddress}")]
        public async Task<IActionResult> GetAll(string macAddress)
        {
            try
            {
                var result = await _repo.GetAllByPeriod(macAddress);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var task = await _repo.GetTaskById(id);

                if (task == null)
                {
                    return NotFound();
                }

                _repo.Delete(task);
                if (await _repo.SaveChangesAsync())
                {
                    return Ok(new { message = "Deletado" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex.Message}");
            }

            return BadRequest("Erro não esperado!!!");
        }

        [HttpPut("{id}/{done}")]
        public async Task<IActionResult> UpdateDone(string id, bool done)
        {
            try
            {
                var result = await _repo.GetTaskById(id);

                if (result == null)
                {
                    return NotFound();
                }

                result.Done = done;
                _repo.Update(result);

                if (await _repo.SaveChangesAsync())
                {
                    return Ok(result);
                }

                return BadRequest("Erro não esperado!!!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex.Message}");
            }
        }

        [HttpGet("filter/late/{macAddress}")]
        public async Task<IActionResult> TaskLate(string macAddress)
        {
            try
            {
                var result = await _repo.GetAllByPeriod(macAddress);

                var late = result.AsQueryable<TaskModel>().Where(t => t.When <= DateTime.UtcNow &&
                    t.Done == false);

                return Ok(late);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex.Message}");
            }
        }

        [HttpGet("filter/today/{macAddress}")]
        public async Task<IActionResult> Today(string macAddress)
        {
            try
            {
                var result = await _repo.GetAllByPeriod(macAddress);

                var today = result.AsQueryable<TaskModel>().Where(t =>
                    t.When.Day == DateTime.UtcNow.Day &&
                    t.When.Month == DateTime.UtcNow.Month &&
                    t.When.Year == DateTime.UtcNow.Year &&
                    t.When >= t.Created &&
                    t.Done == false);

                return Ok(today);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex.Message}");
            }
        }

        [HttpGet("filter/week/{macAddress}")]
        public async Task<IActionResult> Week(string macAddress)
        {
            try
            {
                var result = await _repo.GetAllByPeriod(macAddress);

                int currentWeekNumber = GetWeekNumber(DateTime.UtcNow);

                var week = result.Where(t =>
                    GetWeekNumber(t.When) == currentWeekNumber &&
                    t.When.Year == DateTime.UtcNow.Year &&
                    t.When > DateTime.UtcNow &&
                    t.Done == false);

                return Ok(week);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex.Message}");
            }
        }

        [HttpGet("filter/month/{macAddress}")]
        public async Task<IActionResult> Month(string macAddress)
        {
            try
            {
                var result = await _repo.GetAllByPeriod(macAddress);

                var month = result.AsQueryable<TaskModel>().Where(t =>
                    t.When.Month == DateTime.UtcNow.Month &&
                    t.When.Year == DateTime.UtcNow.Year &&
                    t.When > DateTime.UtcNow &&
                    t.Done == false);

                return Ok(month);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex.Message}");
            }
        }

        [HttpGet("filter/year/{macAddress}")]
        public async Task<IActionResult> Year(string macAddress)
        {
            try
            {
                var result = await _repo.GetAllByPeriod(macAddress);

                var year = result.AsQueryable<TaskModel>().Where(t =>
                    t.When.Year == DateTime.Now.Year &&
                    t.When > DateTime.UtcNow &&
                    t.Done == false);

                return Ok(year);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex.Message}");
            }
        }

        private static int GetWeekNumber(DateTime date)
        {
            System.Globalization.CultureInfo currentCulture = System.Globalization.CultureInfo.CurrentCulture;
            System.Globalization.Calendar currentCalendar = currentCulture.Calendar;

            return
                currentCalendar.GetWeekOfYear(date,
                                              currentCulture.DateTimeFormat.CalendarWeekRule,
                                              currentCulture.DateTimeFormat.FirstDayOfWeek);

        }
    }
}
