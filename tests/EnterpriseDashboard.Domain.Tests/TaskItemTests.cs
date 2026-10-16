using EnterpriseDashboard.Domain.Entities;
using FluentAssertions;

namespace EnterpriseDashboard.Domain.Tests;

public class TaskItemTests
{
    [Fact]
    public void Constructor_WithValidArguments_ShouldCreateTaskWithPendingStatus()
    {
        //Arrange
        string title = "Implement Ingestion Pipeline";
        string description = "Parse CSV data streams using PipeLineReader";
        PriorityLevel priority = PriorityLevel.High;

        //Act
        var taskItem = new TaskItem(title, description, priority);

        //Assert
        taskItem.Id.Should().NotBeEmpty();
        taskItem.Title.Should().Be(title);
        taskItem.Description.Should().Be(description);
        taskItem.Priority.Should().Be(priority);
        taskItem.Status.Should().Be(Entities.TaskStatus.Pending);
        taskItem.CreatedAtUtc.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
        taskItem.UpdatedAtUtc.Should().BeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_WithInvalidTitle_ShouldThrowArgumentException(string? invalidTitle)
    {
        //Act
        var action = () => new TaskItem(invalidTitle!, "description", PriorityLevel.Medium);

        //Assert
        action.Should().Throw<ArgumentException>().WithParameterName("title");
    }

    [Fact]
    public void UpdateStatus_ShouldChangeStatusAndSetupUpdatedAtTimestamp()
    {
        //Arrange
        var taskItem = new TaskItem("Test Task", "description", PriorityLevel.Low);
        DateTime originalCreatedAt = taskItem.CreatedAtUtc;

        //Act
        taskItem.UpdateStatus(Entities.TaskStatus.InProgress);

        //Assert
        taskItem.Status.Should().Be(Entities.TaskStatus.InProgress);
        taskItem.UpdatedAtUtc.Should().NotBeNull();
        taskItem.UpdatedAtUtc.Should().BeAfter(originalCreatedAt.AddMilliseconds(-1));
    }
}
