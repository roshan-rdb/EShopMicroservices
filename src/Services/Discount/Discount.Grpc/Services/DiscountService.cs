using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

public class DiscountService
    (DiscountContext dbContext, ILogger<DiscountService> logger)
    : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
       var coupon = dbContext.Coupones
            .FirstOrDefault(c => c.ProductName == request.ProductName);
        if (coupon == null)
        {
            coupon = new Coupon { ProductName = "No Doscount", Amount = 0 , Description = "No Discount" }; 
        }
        logger.LogInformation("Discount is retrieved for ProductName: {ProductName}, Amount: {Amount}", coupon.ProductName, coupon.Amount);
        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }
    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        if(coupon == null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Coupon data is null"));
        }
        dbContext.Coupones.Add(coupon);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Discount is successfully created. ProductName: {ProductName}", coupon.ProductName);
        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }
    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        if (coupon == null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Coupon data is null"));
        }
        dbContext.Coupones.Update(coupon);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Discount is successfully update. ProductName: {ProductName}", coupon.ProductName);
        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }
    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbContext
            .Coupones
            .FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

        if (coupon == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Discount coupon with product name : {request.ProductName} not found"));
        }

        dbContext.Coupones.Remove(coupon);
        await dbContext.SaveChangesAsync();

        logger.LogInformation($"Discount is successfully deleted for product name {request.ProductName}");

        return new DeleteDiscountResponse { Success = true };

    }
}
