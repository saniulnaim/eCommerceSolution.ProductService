using AutoMapper;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.ServiceContracts;
using DataAccessLayer.Entities;
using DataAccessLayer.RepositoryContacts;
using FluentValidation;
using FluentValidation.Results;
using System.Linq.Expressions;

namespace BusinessLogicLayer.Services
{
    public class ProductService : IProductService
    {
        private readonly IValidator<ProductAddRequest> _productAddRequestValidator;
        private readonly IValidator<ProductUpdateRequest> _productUpdateRequestValidator;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public ProductService(IValidator<ProductAddRequest> productAddRequestValidator,
                              IValidator<ProductUpdateRequest> productUpdateRequestValidator,
                              IMapper mapper,
                              IProductRepository productRepository) 
        {
            _productAddRequestValidator = productAddRequestValidator;
            _productUpdateRequestValidator = productUpdateRequestValidator;
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task<ProductResponse?> AddProduct(ProductAddRequest productAddRequest)
        {
            if (productAddRequest == null)
            {
                throw new ArgumentNullException(nameof(productAddRequest));
            }

            //Validate the product using Fluent Validation
            ValidationResult validationResult = await _productAddRequestValidator.ValidateAsync(productAddRequest);

            // Check the validation result
            if (!validationResult.IsValid)
            {
                string errors = string.Join(", ", validationResult.Errors.Select(temp => temp.ErrorMessage)); //Error1, Error2, ...
                throw new ArgumentException(errors);
            }


            //Attempt to add product
            Product productInput = _mapper.Map<Product>(productAddRequest); //Map productAddRequest into 'Product' type (it invokes ProductAddRequestToProductMappingProfile)
            Product? addedProduct = await _productRepository.AddProduct(productInput);

            if (addedProduct == null)
            {
                return null;
            }

            ProductResponse addedProductResponse = _mapper.Map<ProductResponse>(addedProduct); //Map addedProduct into 'ProductRepsonse' type (it invokes ProductToProductResponseMappingProfile)

            return addedProductResponse;
        }


        public async Task<bool> DeleteProduct(Guid productID)
        {
            Product? existingProduct = await _productRepository.GetProductByCondition(x => x.ProductID == productID);
            if (existingProduct == null)
            {
                return false;
            }

            bool isDeleted = await _productRepository.DeleteProduct(productID); 
            return isDeleted;
        }

        public async Task<ProductResponse?> GetProductByCondition(Expression<Func<Product, bool>> conditionExpression)
        {
            Product? existingProduct = await _productRepository.GetProductByCondition(conditionExpression);
            if (existingProduct == null)
            {
                return null;
            }

            ProductResponse productResponse = _mapper.Map<ProductResponse>(existingProduct);
            return productResponse;
        }

        public async Task<List<ProductResponse?>> GetProducts()
        {
            IEnumerable<Product?> products = await _productRepository.GetProducts();

            IEnumerable<ProductResponse?> productResponses = _mapper.Map<IEnumerable<ProductResponse>>(products);
            return productResponses.ToList();
        }

        public async Task<List<ProductResponse?>> GetProductsByCondition(Expression<Func<Product, bool>> conditionExpression)
        {
            IEnumerable<Product?> products = await _productRepository.GetProductsByCondition(conditionExpression);

            IEnumerable<ProductResponse?> productResponses = _mapper.Map<IEnumerable<ProductResponse>>(products);
            return productResponses.ToList();
        }

        public async Task<ProductResponse?> UpdateProduct(ProductUpdateRequest productUpdateRequest)
        {
            Product? existingProduct = await _productRepository.GetProductByCondition(x => x.ProductID == productUpdateRequest.ProductID);
            if (existingProduct == null)
            {
                throw new ArgumentException($"Product with ID {productUpdateRequest.ProductID} does not exist.");
            }

            //Validate the product using Fluent Validation
            ValidationResult validationResult = await _productUpdateRequestValidator.ValidateAsync(productUpdateRequest);

            // Check the validation result
            if (!validationResult.IsValid)
            {
                string errors = string.Join(", ", validationResult.Errors.Select(temp => temp.ErrorMessage)); //Error1, Error2, ...
                throw new ArgumentException(errors);
            }

            Product product = _mapper.Map<Product>(productUpdateRequest);
            Product? updateProduct = await _productRepository.UpdateProduct(product);

            ProductResponse? updatedProductResponse = _mapper.Map<ProductResponse>(updateProduct);
            return updatedProductResponse;
        }
    }
}
