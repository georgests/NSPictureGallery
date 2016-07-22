using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telligent.Evolution.Extensibility.Api.Entities.Version1;
using Telligent.Evolution.Extensibility.Version1;
using System.Collections;

namespace STSImage.WidgetApi
{
	[Documentation(Category = "Images")]
	public class Images
	{
		[Documentation("Gets a image")]
		public PublicApi.Image Get(
			[Documentation("The image's identifier")]
			Guid id
			)
		{
			return PublicApi.Images.Get(id);
		}

		[Documentation("Deletes a poll answer from a poll.")]
		public AdditionalInfo Delete(
			[Documentation("The poll answer's identifier")]
			Guid id
			)
		{
			return PublicApi.Images.Delete(id);
		}

		[Documentation("Creates a new poll answer within a poll.")]
		public PublicApi.Image Create(
			[Documentation("The identifier for the poll in which to create this answer.")]
			Guid pollId, 
			[Documentation("The name of the answer.")]
			string name
			)
		{
			return PublicApi.Images.Create(pollId, name);
		}

		[Documentation("Update a poll answer.")]
		public PublicApi.Image Update(
			[Documentation("The poll answer's identifier")]
			Guid id, 
			[Documentation(Name="Name", Description="The updated name for the poll answer.", Type=typeof(string))]
			IDictionary options
			)
		{
			string name = null;
			if (options != null && options["Name"] != null)
				name = options["Name"].ToString();

			return PublicApi.Images.Update(id, name);
		}
	}
}
