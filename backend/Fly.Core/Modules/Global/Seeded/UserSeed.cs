using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Global.Dtos;

namespace Fly.Core.Modules.Global.Seeded;

//string[][] users = [["0924951905", "0336330"], ["0915951905", "123456"], ["0924951904", "1"]];
//foreach (var u in users)
//{
//    using var xxx = new HMACSHA512();
//    var saltBytess = xxx.Key;
//    var hashPasswordBytes = xxx.ComputeHash(GetBytes(u[1]));
//    var saltString = To64String(saltBytess);
//    var hashPassword = To64String(hashPasswordBytes);
//    //await Task.Delay(1000);
//}
public class UserSeed
{
    public static List<DtoUser> Users = new List<DtoUser>()
    {
        // 0336330
        new DtoUser()
        {
            Name = "Ghost",
            Sult= "SPcjqk+2nrGlvEnChIh3tMY2adLkzMIGypBaY4h2qcuep5N9wL5lrHekm1EOMaNRARwd67ZotceKY7eNRXXx3nBvmk4lbeG86xxAWZnuuXA3nnmLO//9k7Yx4IQbcLUwOUQOfmJbTKIn5RBYf12gMbOX+EKdPo93C/I2mlX0C2Y=",
            Password = "koad/tT8QGmZmk2B/s2iMALIvKdFe3wfFbwKWtbdhIKPoj2GaAQ/8/lobJxGgYP9cPyb3gfbY+Ery/zS6Mdvew==",
            Phone = "0924951905",
            IsAdmin = true,
            IsGhost = true,
            Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB001"),
        },
        // 123456
        new DtoUser()
        {
            Name = "User",
            Sult  = "/BcPKD/+VMk2vI+8UobrzgSmvizsDgHHPgQzz65YrqH/2fL9nsd9/ju7StpKItqvg5Gxd78Je57NWvWH03aYo9d9pRwYY9sYgJdxmyBzAo7h7vK9G+35WwOI43RPFiIbidOv4fZjhZ8lf5J0+4Fh8l8FvdlIbS0+vb5lga0dxxA=",
            Password= "iD+ZkASu6DmxMy6VVV+DO+EA3s+VEWzYCdmyKRELy8maXGluq2TWqQ0CfAZsHbHScBO8ib31QSHYiSezYTnVWQ==",
            Phone = "0915951905",
            IsAdmin = false,
            IsGhost = false,
            Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB002"),
        },
        // 1
        new DtoUser()
        {
            Name = "Admin",
            Sult  = "sq/uSGqIZcbHYRlFWL81zClVi57NEEoJEx1FH2/lblMBx2YUh4P8BN52wf29UuFoP/6lbnYqpq/I8oyYkf9vPGIOlUUHFy4IKYC3TAi5aZLIUAYu5LDN09gSc72+Jf1e8SwTLST4e4jV0KdV4GvKJw27dcqZS88xzbhe/8zcdKY=",
            Password= "dgNZRDuZ4vtReF151Y3RWLNio5IemIuS3OWYbPsbhfAyq+nqQ+4RQF953hbGpTeXjU4QxG7HsmZj8gB/qDbHuw==",
            Phone = "1",
            IsAdmin = true,
            IsGhost = true,
            Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB003"),
        },
    };
}