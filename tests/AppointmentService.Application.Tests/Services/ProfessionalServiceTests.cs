using AppointmentService.Domain.Models;
using AppointmentService.Domain.Repository;
using AppointmentService.Shared.Dto;
using AppointmentService.Shared.ViewModels;
using AutoMapper;
using FluentAssertions;
using Moq;
using OperationResult;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [Fact]
        public async Task ProfessionalService_WithValidPropertiesThrowAnExceptionOnSave_ShouldBeError()
        {
            // Arrange
            var professional = new Professional
            {
                Id = "1234567890",
                IsEnabled = true,
                ProfilePicture = "http://wwww.image.com/120/image.png",
                Name = "",
                Services = new List<Service>()
            };

            var requestProfessional = new ProfessionalDto
            {
                IsEnabled = true,
                Name = "",
                ProfilePicture = "http://wwww.image.com/120/image.png"
            };

            var responseProfessional = new ProfessionalViewModel
            {
                Id = "1234567890",
                IsEnabled = true,
                ProfilePicture = "http://wwww.image.com/120/image.png",
                Name = "",
                Services = new List<ServiceViewModel>()
            };

            // Action
            _mapper.Setup(m => m.Map<Professional>(requestProfessional)).Returns(professional);

            _factoryProfessionalImp.Setup(fact => fact.Save(professional)).Returns(Result.Error<Professional>(new Exception("Generic error with database")));

            _mapper.Setup(m => m.Map<ProfessionalViewModel>(professional)).Returns(responseProfessional);

            var result = await _sut.CreateNewProfessional(requestProfessional);

            // Assert
            _factoryProfessionalImp.Verify(m => m.Save(professional), Times.Once);
            result.IsSuccess.Should().Be(false);
        }

        [Fact]
        public async Task ProfessionalService_GetAllProfessionals_ShouldBeSuccess()
        {
            // Arrange
            var professionals = new List<Professional>();
            professionals.Add(new Professional
            {
                Id = "1234567890",
                IsEnabled = true,
                ProfilePicture = "http://wwww.image.com/120/image.png",
                Name = "Unit Test Da Silva",
                Services = new List<Service>()
            });

            var responseProfessional = new List<ProfessionalViewModel>();
            responseProfessional.Add(new ProfessionalViewModel
            {
                Id = "1234567890",
                IsEnabled = true,
                ProfilePicture = "http://wwww.image.com/120/image.png",
                Name = "Unit Test Da Silva",
                Services = new List<ServiceViewModel>()
            });

            // Action
            
            _factoryProfessionalImp.Setup(fact => fact.Professionals())
                .Returns(Result.Success(professionals.AsEnumerable()));
            _mapper.Setup(m => m.Map<IEnumerable<ProfessionalViewModel>>(professionals.AsEnumerable())).Returns(responseProfessional.AsEnumerable());

            var result = await _sut.GetAllProfessionals();


            // Assert
            _factoryProfessionalImp.Verify(m => m.Professionals(), Times.Once);
            result.Value.Should().NotBeNull();
            result.Value.Should().HaveCountGreaterThan(0);

        }

        [Fact]
        public async Task ProfessionalService_GetAllProfessionalsThrowAnException_ShouldBeError()
        { 
            // Action

            _factoryProfessionalImp.Setup(fact => fact.Professionals())
                .Returns(Result.Error<IEnumerable<Professional>>(new Exception()));            

            var result = await _sut.GetAllProfessionals();


            // Assert
            _factoryProfessionalImp.Verify(m => m.Professionals(), Times.Once);
            result.Value.Should().BeNullOrEmpty();
            result.IsSuccess.Should().BeFalse();

        }
    }
}
