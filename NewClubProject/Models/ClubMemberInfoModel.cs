using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewClubProject.Models
{
    public class ClubMemberInfoModel
    {
        
        public int ClubId { get; set; }
        public string ClubName { get; set; }
        public string ClubAddress { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public string MemberAddress { get; set; }
        public int NID { get; set; }
        public int MobileNumber { get; set; }
        public int TotalMembers { get; set; }
    }
}