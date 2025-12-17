namespace Skillup_Academy.Helper
{
	public class FileService
	{
		public IFormFile GetDefaultAvatar()
		{
			var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "avatar.jpg");

			var file = new FormFile(System.IO.File.OpenRead(path),
				0, new FileInfo(path).Length, "ClientFile", "avatar.jpg")
			{
				Headers = new HeaderDictionary(),
				ContentType = "image/jpeg"
			};

			return file;
		}
	}
}
