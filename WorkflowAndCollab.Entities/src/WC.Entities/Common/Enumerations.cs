using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareSource.WC.Entities.Common
{
	public enum CompareOperator
	{
		Equal = 0,
		LessThan = 1,
		GreaterThan = 2,
		LessThanEqual = 3,
		GreaterThanEqual = 4,
		Like = 5,
		NotLike = 6,
		NotEqual = 7,
		NotNull = 8,
		Null = 9
	}
}
