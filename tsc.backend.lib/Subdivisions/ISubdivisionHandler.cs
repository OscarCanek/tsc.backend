using System;
namespace tsc.backend.lib.Subdivisions
{
    public interface ISubdivisionHandler : IBaseCrudHandler<Guid, GetSubdivisionDetails, CreateSubdivisionModel, UpdateSubdivisionModel, RemoveSubdivisionModel, SubdivisionModel>
    {
    }
}
