using PatientMonitorDataLogger.PhilipsIntellivue.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

public class MonitorDataGenerator
{
    public Models.List<AttributeValueAssertion> Generate(
        NomenclatureReference objectType,
        OIDType attributeGroup)
    {
        if (objectType == PollObjectTypes.Alerts)
        {
            if (attributeGroup == PollAttributeGroups.Alerts.AlertMonitor)
            {
                return new(
                [
                    // TODO: Add at least all mandatory attributes
                ]);
            }

            if (attributeGroup == PollAttributeGroups.Alerts.VmoStaticContext)
            {
                return new(
                [
                    // TODO: Add at least all mandatory attributes
                ]);
            }

            throw new ArgumentOutOfRangeException(nameof(attributeGroup), $"Unknown attribute group {attributeGroup} for {objectType}");
        }

        if (objectType == PollObjectTypes.MDS)
        {
            if (attributeGroup == PollAttributeGroups.System.SystemIdentification)
            {
                return new(
                [
                    // TODO: Add at least all mandatory attributes
                ]);
            }

            if (attributeGroup == PollAttributeGroups.System.SystemApplication)
            {
                return new(
                [
                    // TODO: Add at least all mandatory attributes
                ]);
            }

            if (attributeGroup == PollAttributeGroups.System.SystemProduction)
            {
                return new(
                [
                    // TODO: Add at least all mandatory attributes
                ]);
            }

            throw new ArgumentOutOfRangeException(nameof(attributeGroup), $"Unknown attribute group {attributeGroup} for {objectType}");
        }

        if(objectType == PollObjectTypes.Numerics)
        {
            if (attributeGroup == PollAttributeGroups.Numerics.VmoStaticContext)
            {
                return new(
                [
                    // TODO: Add at least all mandatory attributes
                ]);
            }

            if (attributeGroup == PollAttributeGroups.Numerics.VmoDynamicContext)
            {
                return new(
                [
                    // TODO: Add at least all mandatory attributes
                ]);
            }

            if (attributeGroup == PollAttributeGroups.Numerics.MetricObservedValue)
            {
                return new(
                [
                    // TODO: Add at least all mandatory attributes
                ]);
            }

            throw new ArgumentOutOfRangeException(nameof(attributeGroup), $"Unknown attribute group {attributeGroup} for {objectType}");
        }

        if(objectType == PollObjectTypes.PatientDemographics)
        {
            if (attributeGroup == PollAttributeGroups.PatientDemographics.All)
            {
                return new(
                [
                    // TODO: Add at least all mandatory attributes
                ]);
            }

            throw new ArgumentOutOfRangeException(nameof(attributeGroup), $"Unknown attribute group {attributeGroup} for {objectType}");
        }

        if(objectType == PollObjectTypes.Waves)
        {
            if (attributeGroup == PollAttributeGroups.Numerics.VmoStaticContext)
            {
                return new(
                [
                    // TODO: Add at least all mandatory attributes
                ]);
            }

            if (attributeGroup == PollAttributeGroups.Numerics.VmoDynamicContext)
            {
                return new(
                [
                    // TODO: Add at least all mandatory attributes
                ]);
            }

            if (attributeGroup == PollAttributeGroups.Numerics.MetricObservedValue)
            {
                return new(
                [
                    // TODO: Add at least all mandatory attributes
                ]);
            }

            throw new ArgumentOutOfRangeException(nameof(attributeGroup), $"Unknown attribute group {attributeGroup} for {objectType}");
        }

        throw new ArgumentOutOfRangeException(nameof(objectType), $"Unknown poll object type {objectType}");
    }
}