namespace Skillup_Academy.AppSettingsImages
{
	public class DeleteImage
	{

		public bool DeleteImg(string fileName)
		{
			string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img");

			string fullPath = Path.Combine(folderPath, fileName);

			if (File.Exists(fullPath))
			{

				File.Delete(fullPath);

				return true;
			}
			else
			{
				return false;
			}
		}

	}
}
