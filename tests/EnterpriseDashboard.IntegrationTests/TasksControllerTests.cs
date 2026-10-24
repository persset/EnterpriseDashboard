using System;
using System.Net.Http.Json;
using EnterpriseDashboard.Application.DTOs.Tasks;
using EnterpriseDashboard.Domain.Entities;
using FluentAssertions;

namespace EnterpriseDashboard.IntegrationTests;

public class TasksControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public TasksControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllTasks_ReturnEmptyList_WhenNoTaskExist()
    {
        // When
        var response = await _client.GetAsync("/api/tasks");

        // Then
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var tasks = await response.Content.ReadFromJsonAsync<List<TaskItem>>();
        tasks.Should().NotBeNull();
        tasks.Should().BeEmpty();
    }

    [Fact]
    public async Task CreateTask_ReturnsCreatedTask_WithValidPayload()
    {
        // Given
        var newTaskDto = new CreateTaskDto(
            Title: "Integration test task",
            Description: "Integration testing",
            Priority: (int)PriorityLevel.High,
            DueDateUtc: DateTime.Now.AddDays(3)
        );

        // When
        var response = await _client.PostAsJsonAsync("api/tasks", newTaskDto);

        // Then
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var createdTask = await response.Content.ReadFromJsonAsync<TaskItem>();
        createdTask.Should().NotBeNull();
        createdTask.Title.Should().Be("Integration test task");
        createdTask.Priority.Should().Be(PriorityLevel.High);

        response.Headers.Location.Should().NotBeNull();
    }
}
