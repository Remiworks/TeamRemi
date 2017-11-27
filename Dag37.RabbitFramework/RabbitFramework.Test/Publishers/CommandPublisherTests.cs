﻿using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using RabbitFramework.Contracts;
using RabbitFramework.Models;
using RabbitFramework.Publishers;
using Shouldly;

namespace RabbitFramework.Test.Publishers
{
    [TestClass]
    public class CommandPublisherTests
    {
        private const string QueueNameParamName = "queueName";
        private const string MessageParamName = "message";
        private const string KeyParamName = "key";

        private const string Key = "testKey";
        private const string QueueName = "testQueue";
        private const int Timeout = 0;

        private readonly Mock<IBusProvider> _busProviderMock = new Mock<IBusProvider>(MockBehavior.Strict);

        private CommandPublisher _sut;

        private EventReceivedCallback _eventReceivedCallback = (_) =>
            throw new NotImplementedException("This callback is not yet set by IBusProvider.BasicConsume");

        [TestInitialize]
        public void TestInitialize()
        {
            _busProviderMock
                .Setup(b => b.BasicConsume(It.IsAny<string>(), It.IsAny<EventReceivedCallback>()))
                .Callback<string, EventReceivedCallback>((_, callback) => _eventReceivedCallback = callback);

            _sut = new CommandPublisher(_busProviderMock.Object);
        }

        //[TestMethod]
        //public void SendCommandCallsBasicPublishWithCorrectParameters()
        //{
        //    _busProviderMock
        //        .Setup(b => b.CreateTopicsForQueue(It.IsAny<string>(), It.IsAny<string[]>()));

        //    _busProviderMock
        //        .Setup(b => b.BasicPublish(It.Is(CorrectEventMessage)))
        //        .Callback(BasicPublishCallback)
        //        .Verifiable();

        //    _sut.SendCommandAsync(_message, QueueName, Key).Wait();

        //    _busProviderMock.VerifyAll();
        //}

        //[TestMethod]
        //public void SendCommandReturnsCorrectResult()
        //{
        //    _busProviderMock
        //        .Setup(b => b.BasicPublish(It.Is(CorrectEventMessage)))
        //        .Callback(BasicPublishCallback)
        //        .Verifiable();

        //    var result = _sut.SendCommandAsync(_message, QueueName, Key).Result;

        //    result.ShouldBe();
        //}

        //[TestMethod]
        //public void SendCommandThrowsArgumentNullExceptionWhenQueueNameIsNull()
        //{
        //    var exception = Should.Throw<ArgumentNullException>(() => _sut.SendCommand<string>(new object(), null, Key, Timeout));
        //    exception.ParamName.ShouldBe(QueueNameParamName);
        //}

        //[TestMethod]
        //public void SendCommandThrowsArgumentNullExceptionWhenQueueNameIsEmpty()
        //{
        //    var exception = Should.Throw<ArgumentNullException>(() => _sut.SendCommand<string>(new object(), "", Key, Timeout));
        //    exception.ParamName.ShouldBe(QueueNameParamName);
        //}

        //[TestMethod]
        //public void SendCommandThrowsArgumentNullExceptionWhenQueueNameIsWhitespace()
        //{
        //    var exception = Should.Throw<ArgumentNullException>(() => _sut.SendCommand<string>(new object(), " ", Key, Timeout));
        //    exception.ParamName.ShouldBe(QueueNameParamName);
        //}

        //[TestMethod]
        //public void SendCommandThrowsArgumentNullExceptionWhenMessageIsNull()
        //{
        //    var exception = Should.Throw<ArgumentNullException>(() => _sut.SendCommand<string>(null, QueueName, Key, Timeout));
        //    exception.ParamName.ShouldBe(MessageParamName);
        //}

        //[TestMethod]
        //public void SendCommandThrowsArgumentNullExceptionWhenKeyIsNull()
        //{
        //    var exception = Should.Throw<ArgumentNullException>(() => _sut.SendCommand<string>(new object(), QueueName, null, Timeout));
        //    exception.ParamName.ShouldBe(KeyParamName);
        //}

        //[TestMethod]
        //public void SendCommandThrowsArgumentNullExceptionWhenKeyIsEmpty()
        //{
        //    var exception = Should.Throw<ArgumentNullException>(() => _sut.SendCommand<string>(new object(), QueueName, "", Timeout));
        //    exception.ParamName.ShouldBe(KeyParamName);
        //}

        //[TestMethod]
        //public void SendCommandThrowsArgumentNullExceptionWhenKeyIsWhitespace()
        //{
        //    var exception = Should.Throw<ArgumentNullException>(() => _sut.SendCommand<string>(new object(), QueueName, " ", Timeout));
        //    exception.ParamName.ShouldBe(KeyParamName);
        //}

        //[TestMethod]
        //public void SendCommandThrowsArgumentExceptionWhenKeyContainsWildCardStar()
        //{
        //    var exception = Should.Throw<ArgumentException>(() => _sut.SendCommand<string>(new object(), QueueName, "test.*.event", Timeout));
        //    exception.Message.ShouldBe("Key may not contain wildcards");
        //}

        //[TestMethod]
        //public void SendCommandThrowsArgumentExceptionWhenKeyContainsWildCardHashtag()
        //{
        //    var exception = Should.Throw<ArgumentException>(() => _sut.SendCommand<string>(new object(), QueueName, "test.#.event", Timeout));
        //    exception.Message.ShouldBe("Key may not contain wildcards");
        //}

        //private Expression<Func<EventMessage, bool>> CorrectEventMessage =>
        //    eventMessage =>
        //        eventMessage.RoutingKey == Key &&
        //        //eventMessage.JsonMessage == JsonConvert.SerializeObject(_message);

        private Action<EventMessage> BasicPublishCallback =>
            receivedEvent =>
            {
                _eventReceivedCallback(new EventMessage
                {
                    CorrelationId = receivedEvent.CorrelationId,
                    JsonMessage = receivedEvent.JsonMessage,
                    RoutingKey = receivedEvent.RoutingKey,
                    Type = receivedEvent.Type
                });
            };
    }
}