namespace Skillup_Academy.AppSettingsImages
{
	public class SaveImage
	{
		private readonly IWebHostEnvironment _host;

		public SaveImage(IWebHostEnvironment host)
		{
			_host = host;
		}


		public async Task<string> SaveImgAsync(IFormFile imageFile)
		{
			if (imageFile.Length > 0)
			{
				string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);

				string folderPath = Path.Combine(_host.WebRootPath, "img"); //wwwroot/img
				string fullPath = Path.Combine(folderPath, fileName);      //wwwroot/img/اسم_الملف.jpg

				if (!Directory.Exists(folderPath))
					Directory.CreateDirectory(folderPath);

				using (var stream = new FileStream(fullPath, FileMode.Create))
				{
					await imageFile.CopyToAsync(stream);
				}

				return fileName;
			}

			return null;
		}
	}
}
