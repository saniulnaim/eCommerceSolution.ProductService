using BusinessLogicLayer.DTO;
using BusinessLogicLayer.ServiceContracts;
using FluentValidation;

namespace ProductMicroService.API.APIEndpoints
{
    public static class ProductAPIEndpoints
    {
        public static IEndpointRouteBuilder MapProductAPIEndpoints(this IEndpointRouteBuilder app)
        {
            // GET /api/products
            app.MapGet("/api/products", async (IProductService productService) =>
            {
                List<ProductResponse?> products = await productService.GetProducts();
                return Results.Ok(products);
            });

            // GET /api/products/search/product-id/{ProductID:guid}
            app.MapGet("/api/products/search/product-id/{ProductID:guid}", async (IProductService productService, Guid ProductID) =>
            {
                ProductResponse? product = await productService.GetProductByCondition(p => p.ProductID == ProductID);
                return product is not null ? Results.Ok(product) : Results.NotFound();
            });

            // GET /api/products/search/xxxxx
            app.MapGet("/api/products/search/{SearchString}", async (IProductService productService, string SearchString) =>
            {
                List<ProductResponse?> productsByProductName = await productService.GetProductsByCondition(p => p.ProductName != null && p.ProductName.Contains(SearchString));

                List<ProductResponse?> productsByCategory = await productService.GetProductsByCondition(p => p.Category != null && p.Category.Contains(SearchString));

                var products = productsByProductName.Union(productsByCategory);
                return Results.Ok(products);
            });

            // POST /api/products
            app.MapPost("/api/products", async (IProductService productService, IValidator<ProductAddRequest> productValidator, ProductAddRequest productAddRequest) =>
            {
                var validationResult = await productValidator.ValidateAsync(productAddRequest);
                if (!validationResult.IsValid)
                {
                    Dictionary<string, string[]> errors = validationResult.Errors.GroupBy(x => x.PropertyName).ToDictionary(grp => grp.Key, grp => grp.Select(err => err.ErrorMessage).ToArray());
                    return Results.ValidationProblem(errors);
                }

                ProductResponse? addedProduct = await productService.AddProduct(productAddRequest);
                return addedProduct is not null ? Results.Created($"/api/products.search/product-id/{addedProduct.ProductID}", addedProduct) : Results.Problem("Error in adding product");
            });

            // PUT /api/products
            app.MapPut("/api/products", async (IProductService productService, IValidator<ProductUpdateRequest> productValidator, ProductUpdateRequest productUpdateRequest) =>
            {
                var validationResult = await productValidator.ValidateAsync(productUpdateRequest);
                if (!validationResult.IsValid)
                {
                    Dictionary<string, string[]> errors = validationResult.Errors.GroupBy(x => x.PropertyName).ToDictionary(grp => grp.Key, grp => grp.Select(err => err.ErrorMessage).ToArray());
                    return Results.ValidationProblem(errors);
                }

                ProductResponse? updatedProduct = await productService.UpdateProduct(productUpdateRequest);
                return updatedProduct is not null ? Results.Ok(updatedProduct) : Results.Problem("Error in updating product");
            });

            // DELETE /api/products/{ProductID:guid}
            app.MapDelete("/api/products/{ProductID:guid}", async (IProductService productService, Guid ProductID) =>       
            {
                bool isDeleted = await productService.DeleteProduct(ProductID);
                return isDeleted ? Results.Ok(true) : Results.Problem("Error in deleting product");
            });

            return app;
        }
    }
}
