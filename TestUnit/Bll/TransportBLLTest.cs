using API.Model;
using BLL.Dto;
using BLL.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestUnit.Bll
{

    public class TransportBLLTest
    {
        private readonly IBusinessLogic<TransportDTO> _transportBLL;
        private readonly Mock<IBusinessLogic<TransportDTO>> _transportBLLMock;

        public TransportBLLTest(Mock<IBusinessLogic<TransportDTO>> transportBLLMock, IBusinessLogic<TransportDTO> transportBLL)
        {
            _transportBLLMock = transportBLLMock;
            _transportBLL = transportBLL;

        }

        [Fact]
        public async Task InsertOneAsync()
        {
            //Arrange
            TransportDTO transport = new TransportDTO();
            //Act
            var result = ((ITransport)_transportBLL).Crear(transport);

            //Assert
            _transportBLLMock
                .Verify(
                mock => mock.Crear(
                    It.IsAny<TransportDTO>()),
                    Times.Once());

            Assert.IsTrue(result > 1);

        }



    }
}
