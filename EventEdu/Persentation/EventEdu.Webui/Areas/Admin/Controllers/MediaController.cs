//using EventEdu.Application.DTOs.Media;
//using EventEdu.Application.Services;
//using EventEdu.Persistence.Services;
//using Microsoft.AspNetCore.Mvc;


//namespace EventEdu.Webui.Areas.Admin.Controllers
//{
//    [Area("Admin")]
//    public class MediaController : Controller
//    {
//        private readonly IMediaService _mediaService;
//        private readonly IWebHostEnvironment _env;
//        public MediaController(IMediaService mediaService, IWebHostEnvironment env)
//        {
//            _mediaService = mediaService;
//            _env = env;
//        }

//        public IActionResult Index()
//        {
//            return View();
//        }

       

//        [HttpPost]
//        public async Task<IActionResult> UploadMedia([FromForm] CreateMediaDTO createMediaDTO)
//        {
//            try
//            {
//                await _mediaService.AddMediaAsync(createMediaDTO, _env.WebRootPath);
//                return Ok("File uploaded successfully.");
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }
//        [HttpPost]
//        public async Task<IActionResult> DeleteMedia(Guid id)
//        {
//                try
//                {
//                    string webRootPath = _env.WebRootPath;
//                    await _mediaService.DeleteMedia(id, webRootPath);
//                    return RedirectToAction(nameof(Index));
//                }
//                catch (KeyNotFoundException ex)
//                {
//                    //TempData["Error"] = ex.Message;
//                    return RedirectToAction(nameof(Index));
//                }
//        }
//    }
//}
