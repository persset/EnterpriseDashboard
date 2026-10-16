using System;
using EnterpriseDashboard.Domain.Entities;
using FluentAssertions;

namespace EnterpriseDashboard.Domain.Tests;

public class IngestionBatchTests
{
    [Fact]
    public void Constructor_WithValidArguments_ShouldCreateBatchWithQueuedStatus()
    {
        //Arrange
        string fileName = "dados vendas.csv";
        long fileSize = 10485676L;
        int totalRecords = 5000;

        //Act
        var batch = new IngestionBatch(fileName, fileSize, totalRecords);

        //Assert
        batch.Id.Should().NotBeEmpty();
        batch.FileName.Should().Be(fileName);
        batch.FileSizeBytes.Should().Be(fileSize);
        batch.TotalRecords.Should().Be(totalRecords);
        batch.ProcessedRecords.Should().Be(0);
        batch.ErrorSummary.Should().BeNull();
        batch.CreatedAtUtc.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
        batch.UpdatedAtUtc.Should().BeNull();
    }

    [Fact]
    public void MarkProcessing_ShouldChangeStatusToProcessing()
    {
        //Arrange
        var batch = new IngestionBatch("data.csv", 500, 100);

        //Act
        batch.MarkProcessing();

        //Assert
        batch.Status.Should().Be(IngestionStatus.Processing);
        batch.UpdatedAtUtc.Should().NotBeNull();
        batch.UpdatedAtUtc.Should().BeAfter(batch.CreatedAtUtc.AddMilliseconds(-1));
    }

    [Fact]
    public void ProgressRecordCount_ShouldIncrementProcessRecords()
    {
        //Arrange
        var batch = new IngestionBatch("data.csv", 500, 100);
        batch.MarkProcessing();

        //Act
        batch.ProgressRecordCount(45);
        batch.ProgressRecordCount(55);

        //Assert
        batch.ProcessedRecords.Should().Be(100);
        batch.UpdatedAtUtc.Should().NotBeNull();
        batch.UpdatedAtUtc.Should().BeAfter(batch.CreatedAtUtc.AddMilliseconds(-1));
    }

    [Fact]
    public void Complete_ShouldChangeStatusToCompleted()
    {
        //Arrange
        var batch = new IngestionBatch("data.csv", 500 ,100);
        batch.MarkProcessing();
        batch.ProgressRecordCount(100);

        //Act
        batch.Complete();

        //Assert
        batch.Status.Should().Be(IngestionStatus.Completed);
        batch.UpdatedAtUtc.Should().NotBeNull();
        batch.UpdatedAtUtc.Should().BeAfter(batch.CreatedAtUtc.AddMilliseconds(-1));
    }

    [Fact]
    public void TestName()
    {
        //Arrange
        var batch = new IngestionBatch("data.csv", 500, 100);
        string errorMessage = "Malformed CSV header detected in line 17.";

        //Act
        batch.Fail(errorMessage);

        //Assert
        batch.Status.Should().Be(IngestionStatus.Failed);
        batch.ErrorSummary.Should().Be(errorMessage);
        batch.UpdatedAtUtc.Should().NotBeNull();
        batch.UpdatedAtUtc.Should().BeAfter(batch.CreatedAtUtc.AddMilliseconds(-1));
    }
}
