using EAI.SAPNco.IDOC;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EAI.SAPNco.IDOC.Json
{
    public class JIdocBuilder
    {
        public static JObject CreateJIdoc(Idoc idoc)
        {
            var jSegments =
                        CreateJSegments(idoc)
                            .ToArray();

            return new JObject(
                    new JProperty(nameof(JIdoc.DC40), JObject.FromObject(idoc.DC40)),
                    jSegments
                );
        }

        public static IEnumerable<JProperty> CreateJSegments(IIdocSegmentContainer idocSegmentContainer)
        {
            var childSegmentsMetadata = idocSegmentContainer.ChildSegmentsMetadata.ToArray();
            var segments = idocSegmentContainer
                            .ChildSegments
                            .ToArray();

            foreach(var childSegmentMetadata in childSegmentsMetadata)
            {
                var metadataSegments = segments.Where(s => s.Name == childSegmentMetadata.Name);

                if(childSegmentMetadata.IsGroup)
                {
                    var jGroups = metadataSegments.Select(s => CreateJGroup(s));

                    var jGroupProperty = new JProperty($"{childSegmentMetadata.Name}GRP",
                            childSegmentMetadata.MaxGroupOccurrence > 1 ?
                                (JToken)new JArray(jGroups) :
                                jGroups.FirstOrDefault()
                        );

                    yield return jGroupProperty;
                }
                else
                {
                    var jSegments = metadataSegments.Select(s => CreateJSegment(s));

                    var jProperty = new JProperty(childSegmentMetadata.Name,
                            childSegmentMetadata.MaxOccurrence > 1 ?
                                (JToken)new JArray(jSegments) :
                                jSegments.FirstOrDefault()
                        );

                    yield return jProperty;
                }
            }
        }

        public static JObject CreateJGroup(IdocSegment idocSegment)
            => new JObject()
                    {
                        new JProperty(idocSegment.Name, CreateJSegment(idocSegment)),

                        CreateJSegments(idocSegment)
                    };

        public static JObject CreateJSegment(IdocSegment idocSegment)
            => new JObject(
                    new JProperty("Segment_Name", idocSegment.Name),
                    new JProperty("Segment_Type", idocSegment.Metadata.Type),
                    new JProperty("Segment_RefType", idocSegment.Metadata.Type),
                    new JProperty("Segment_Number", idocSegment.Number),
                    new JProperty("Segment_ParentNumber", idocSegment.ParentNumber),
                    new JProperty("Segment_HierarchyLevel", idocSegment.HierarchyLevel),

                    idocSegment
                            .Fields
                            .Select(f => new JProperty(f.Metadata.Name, f.Value))
                );
    }
}
