
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var request = controllerContext.HttpContext.Request;
            var id = request.Form.Get("Id");
            var name = request.Form.Get("Name");
            if (!validUserIDN(name))
            {
                var d = new ModelStateDictionary();
                bindingContext.ModelState.AddModelError("name", "身份證錯誤");
                return base.BindModel(controllerContext, new ModelBindingContext
                {
                    ModelMetadata = bindingContext.ModelMetadata,
                    ModelState = bindingContext.ModelState,
                    PropertyFilter = bindingContext.PropertyFilter,
                    ValueProvider = bindingContext.ValueProvider
                });
            }
            return base.BindModel(controllerContext, bindingContext);
        }
