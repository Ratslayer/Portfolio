using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mongoose
{
	public interface IFileAccessor
	{
		Task SerializeAndSaveObjects(List<Event> pointsOfInterest,string filename);
		Task<string> LoadSerializedObjects(string filename);
	}
}

