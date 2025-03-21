using Moq;
using System.Net;
using Fase03.Domain.Exceptions;

namespace TechChallengeFase01.Tests.Middlewares
{
   /* public class ExceptionMiddlewareTests
    {
        private Mock<RequestDelegate> _requestDelegateMock;

        public ExceptionMiddlewareTests()
        {
            _requestDelegateMock = new Mock<RequestDelegate>();
        }

        [Fact(DisplayName = "Retornar Bad Request quando for lançado um Repository Exception")]
        public async Task InvokeAsync_When_Throws_Repository_Exception_Returns_Bad_Request()
        {
            // Arrange
            var context = new DefaultHttpContext();
            _requestDelegateMock.Setup(rd => rd.Invoke(context))
                .ThrowsAsync(new RepositoryException("Repository error", new Exception()));

            var middleware = new ExceptionMiddleware(_requestDelegateMock.Object);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, context.Response.StatusCode);
        }

        [Fact(DisplayName = "Retornar Bad Request quando for lançado um Domain Exception")]
        public async Task InvokeAsync_When_Throws_Domain_Exception_Returns_Bad_Request()
        {
            // Arrange
            var context = new DefaultHttpContext();
            _requestDelegateMock.Setup(rd => rd.Invoke(context))
                .ThrowsAsync(new DomainException("Domain error"));

            var middleware = new ExceptionMiddleware(_requestDelegateMock.Object);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, context.Response.StatusCode);
        }

        [Fact(DisplayName = "Retornar Bad Request quando for lançada um Application Exception")]
        public async Task InvokeAsync_When_Throws_Application_Exception_Returns_Bad_Request()
        {
            // Arrange
            var context = new DefaultHttpContext();
            _requestDelegateMock.Setup(rd => rd.Invoke(context))
                .ThrowsAsync(new ApplicationException("Application error"));

            var middleware = new ExceptionMiddleware(_requestDelegateMock.Object);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, context.Response.StatusCode);
        }

        [Fact(DisplayName = "Retornar Internal Server Error quando for lançada uma Exception")]
        public async Task InvokeAsync_When_Throws_Exception_Returns_Internal_Server_Error()
        {
            // Arrange
            var context = new DefaultHttpContext();
            _requestDelegateMock.Setup(rd => rd.Invoke(context))
                .ThrowsAsync(new Exception("Generic error"));

            var middleware = new ExceptionMiddleware(_requestDelegateMock.Object);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
        }
    }*/
}
