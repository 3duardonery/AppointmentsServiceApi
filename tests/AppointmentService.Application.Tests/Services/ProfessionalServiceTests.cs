using AppointmentService.Domain.Models;
using AppointmentService.Domain.Repository;
using AppointmentService.Shared.Dto;
using AppointmentService.Shared.ViewModels;
using AutoMapper;
using FluentAssertions;
using Moq;
using OperationResult;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace AppointmentService.Application.Services
{
    public sealed class ProfessionalServiceTests
    {
        private readonly Mock<FactoryProfessionalImp> _factoryProfessionalImp;
        private readonly Mock<FactoryProfessionalServicesImp> _factoryProfessionalServicesImp;
        private readonly Mock<IMapper> _mapper;
        private readonly ProfessionalService _sut;

        public ProfessionalServiceTests()
        {
            _factoryProfessionalImp = new Mock<FactoryProfessionalImp>();
            _factoryProfessionalServicesImp = new Mock<FactoryProfessionalServicesImp>();
            _mapper = new Mock<IMapper>();
            _sut = new ProfessionalService(
                _factoryProfessionalImp.Object, _factoryProfessionalServicesImp.Object, _mapper.Object);
        }

        [Fact]
        public async Task ProfessionalService_WithAllValidProperties_ShouldBeSuccess()
        {
            // Arrange
            var professional = new Professional 
            {
                Id = "1234567890",
                IsEnabled = true,
                ProfilePicture = "http://wwww.image.com/120/image.png",
                Name = "Unit Test Da Silva",
                Services = new List<Service>()
            };

            var requestProfessional = new ProfessionalDto
            {
                IsEnabled = true,
                Name = "Unit Test Da Silva",
                ProfilePicture = "http://wwww.image.com/120/image.png"
            };

            var responseProfessional = new ProfessionalViewModel 
            {
                Id = "1234567890",
                IsEnabled = true,
                ProfilePicture = "http://wwww.image.com/120/image.png",
                Name = "Unit Test Da Silva",
                Services = new List<ServiceViewModel>()
            };

            // Action
            _mapper.Setup(m => m.Map<Professional>(requestProfessional)).Returns(professional);
            _factoryProfessionalImp.Setup(fact => fact.Save(professional))
                .Returns(Result.Success(professional));
            _mapper.Setup(m => m.Map<ProfessionalViewModel>(professional)).Returns(responseProfessional);

            var result = await _sut.CreateNewProfessional(requestProfessional);


            // Assert
            _factoryProfessionalImp.Verify(m => m.Save(professional), Times.Once);
            result.Value.Should().NotBeNull();
            result.Value.Id.Should().Be(professional.Id);
            
        }
    }
}
