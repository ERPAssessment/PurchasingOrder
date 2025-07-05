using PurchasingOrder.Application.PurchaseOrders.Commands.ApprovePurchaseOrder;
using PurchasingOrder.Domain.Abstractions;
using PurchasingOrder.Infrastructure.Data;
using System.Reflection;

namespace PurchasingOrder.ArchitectureTests.Infrastructure;

public class BaseTest
{
  protected static readonly Assembly ApplicationAssembly = typeof(ApprovePurchaseOrderCommand).Assembly;

  protected static readonly Assembly DomainAssembly = typeof(IEntity).Assembly;

  protected static readonly Assembly InfrastructureAssembly = typeof(ApplicationDbContext).Assembly;

  protected static readonly Assembly PresentationAssembly = typeof(Program).Assembly;
}