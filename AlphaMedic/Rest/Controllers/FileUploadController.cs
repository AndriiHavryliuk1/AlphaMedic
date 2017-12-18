using System;
using System.Linq;
using System.Web.Http;
using System.Threading.Tasks;
using Rest.Models;
using System.Net.Http;
using System.IO;
using System.Net;
using Rest.Models.AlphaMedicContext;

namespace Rest.Controllers
{

	public class FilenameMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
	{
		public FilenameMultipartFormDataStreamProvider(string path) : base(path)
		{
		}

		public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
		{

			return string.Format(@"{0}.jpg", DateTime.Now.Ticks);
		}
	}

	[Authorize]
	[RoutePrefix("api/image")]
	public class FileUploadController : ApiController
	{
		private static readonly string ServerUploadFolder = System.Web.Hosting.HostingEnvironment.MapPath("~/img");

		private AlphaMedicContext db = new AlphaMedicContext();
		//For user
		[Route("{id:int}")]
		[HttpPost]
		public async Task<IHttpActionResult> UploadFile(int id)
		{


			HttpRequestMessage request = this.Request;
			if (!request.Content.IsMimeMultipartContent())
			{
				throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
			}
			try
			{
				var currentUser = db.Users.FirstOrDefault(x => x.Email == this.User.Identity.Name);

				if (currentUser == null)
				{
					return NotFound();
				}

				if (!this.User.IsInRole(Roles.Administrator) && id != currentUser.UserId)
				{
					return Content(HttpStatusCode.Forbidden, Messages.AccsesDenied);
				}

				var streamProvider = new FilenameMultipartFormDataStreamProvider(ServerUploadFolder);
				await Request.Content.ReadAsMultipartAsync(streamProvider);


				var fileName = Path.GetFileName(streamProvider.FileData.Select(entry => entry.LocalFileName).First());

				var user = db.Users.Find(id);

				if (user == null)
				{
					return NotFound();
				}

				user.URLImage = "img/" + fileName;

				db.Entry(user).State = System.Data.Entity.EntityState.Modified;

				db.SaveChanges();
				return Ok(Constants.ThisServer + "img/" + fileName);
			}
			catch (Exception ex)
			{
				return InternalServerError(ex);
			}


		}
		[Route("department/{id:int}")]
		[HttpPost]
		[Authorize(Roles = Roles.Administrator)]
		public async Task<IHttpActionResult> UploadDepFile(int id)
		{


			HttpRequestMessage request = this.Request;
			if (!request.Content.IsMimeMultipartContent())
			{
				throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
			}
			try
			{
				var streamProvider = new FilenameMultipartFormDataStreamProvider(ServerUploadFolder);
				await Request.Content.ReadAsMultipartAsync(streamProvider);


				var fileName = Path.GetFileName(streamProvider.FileData.Select(entry => entry.LocalFileName).First());

				var dep = db.Departments.Find(id);

				if (dep == null)
				{
					return NotFound();
				}

				dep.URLImage = "img/" + fileName;

				db.Entry(dep).State = System.Data.Entity.EntityState.Modified;

				db.SaveChanges();
				return Ok(Constants.ThisServer + "img/" + fileName);
			}
			catch (Exception ex)
			{
				return InternalServerError(ex);
			}


		}


	}
}
