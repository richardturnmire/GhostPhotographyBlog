﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GhostPhotographerBlog.Models.Queries;

namespace GhostPhotographerBlog.UI.Models {
    public class AdminPageModel {
        public List<UserRoles> Users { get; set; }
        public List<BlogPostInfo> Posts { get; set; }
    }
}