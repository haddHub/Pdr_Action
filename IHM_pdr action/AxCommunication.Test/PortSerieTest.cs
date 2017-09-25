using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moq;

namespace AxCommunication.Test
{
    [TestFixture]
    public class PortSerieTest
    {
        private Mock<IPortSerieService> mockPort;

        [SetUp]
        public void Init()
        {
            mockPort = new Mock<IPortSerieService>();

        }

        [Test]
        public void Deconnexion_Robot_Correcte()
        {
            mockPort.Setup(m => m.Open());
            mockPort.Object.Close();
            Assert.IsFalse(mockPort.Object.IsOpen());
        }

        [Test]
        public void Connexion_Robot_Correcte()
        {
            mockPort.Setup(m => m.Open());
            Assert.IsFalse(mockPort.Object.IsOpen());
        }
    }
}
