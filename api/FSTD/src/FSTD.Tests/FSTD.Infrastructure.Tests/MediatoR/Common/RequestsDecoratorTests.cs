using FSTD.Infrastructure.MediatoR.Common.Decorators;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace FSTD.Infrastructure.Unit.Tests.MediatoR.Common
{
    public class RequestsDecoratorTests
    {
        private readonly Mock<ILogger<RequestsDecorator<TestRequest, TestResponse>>> _loggerMock;
        private readonly RequestsDecorator<TestRequest, TestResponse> _decorator;
        private readonly Mock<RequestHandlerDelegate<TestResponse>> _nextMock;

        public RequestsDecoratorTests()
        {
            _loggerMock = new Mock<ILogger<RequestsDecorator<TestRequest, TestResponse>>>();
            _nextMock = new Mock<RequestHandlerDelegate<TestResponse>>();

            _decorator = new RequestsDecorator<TestRequest, TestResponse>(_loggerMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Log_Start_And_End_Time()
        {
            // Arrange
            var request = new TestRequest();
            var response = new TestResponse();
            _nextMock.Setup(n => n()).ReturnsAsync(response);

            // Act
            var result = await _decorator.Handle(request, _nextMock.Object, CancellationToken.None);

            // Assert
            Assert.Equal(response, result);

            // Verify that the logger logged the start message
            _loggerMock.Verify(
                x => x.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Request name:{typeof(TestRequest).Name}, Start DateTime")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once);

            // Verify that the logger logged the end message
            _loggerMock.Verify(
                x => x.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Request name:{typeof(TestRequest).Name}, End DateTime")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_TaskCanceledException_When_Task_Is_Canceled()
        {
            // Arrange
            var request = new TestRequest();
            _nextMock.Setup(n => n()).ThrowsAsync(new TaskCanceledException());

            // Act & Assert
            var ex = await Assert.ThrowsAsync<TaskCanceledException>(() => _decorator.Handle(request, _nextMock.Object, CancellationToken.None));

            Assert.Equal($"The task: {typeof(TestRequest).Name} was cancelled", ex.Message);
        }
    }

    public class TestRequest : IRequest<TestResponse>
    {
        public string TestProperty { get; set; }
    }

    public class TestResponse
    {
        public string ResponseProperty { get; set; }
    }

}
