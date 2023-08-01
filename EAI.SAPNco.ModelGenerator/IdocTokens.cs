using EAI.Dataverse.ModelGenerator;
using EAI.OnPrem.SAPNcoService;
using EAI.OnPrem.Storage;
using EAI.SAPNco.IDOC.Metadata;
using EAI.SAPNco.Model;
using EAI.SAPNco.ModelGenerator.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EAI.SAPNco.ModelGenerator
{
    public class IdocTokens
    {
        private Dictionary<string, IToken> _rfcSegmentTokens = new Dictionary<string, IToken>();

        public string Namespace { get; set; }

        public async Task<IEnumerable<IToken>> GetIdocTokensAsync(Assembly modelAssembly, OnPremClient onPremClient, string sapSystemName, IEnumerable<IdocName> idocNames)
        {
            var view = new ModelAssemblyView() { ModelAssembly = modelAssembly };

            Namespace = view.Types.FirstOrDefault()?.Namespace ?? "EAI.SAPNco.Model";

            _rfcSegmentTokens.Clear();

            var rfcClient = new RfcGatewayServiceProxy();
            rfcClient.OnPremClient = onPremClient;

            var tokens = new List<IToken>();

            foreach (var idocName in idocNames)
            {
                var idocMetadata = await IdocMetadataCache.GetIdocMetadataAsync(rfcClient, sapSystemName, idocName.IdocTyp, idocName.CimTyp, idocName.DocRel);

                tokens.Add(new IdocToken()
                {
                    IdocName = $"IDOC_{idocMetadata.Header.IDOCTYP}_{idocMetadata.Header.CIMTYP}_{idocMetadata.Header.RELEASED}",
                    Namespace = Namespace,
                    ChildTokens = idocMetadata.RootSegments.Select(s => CreateSegmentProperty(s)).ToArray()
                });
                
                foreach(var segement in idocMetadata.Segments)
                {
                    if (segement.IsGroup)
                    {
                        var segemtGroupToken = new IdocSegmentGroupToken()
                        {
                            SegmentName = segement.Name,
                            Namespace = Namespace,
                            ChildTokens = 
                                    new[] { new PropertyToken()
                                        {
                                            CSharpType = $"{segement.Name}",
                                            PropertyName = $"{segement.Name}",
                                            Description = segement.Description
                                        }
                                    }
                                    .Union(
                                            segement
                                                .ChildSegments
                                                .Select(s => CreateSegmentProperty(s))
                                    )
                                    .ToArray()
                        };

                        tokens.Add(segemtGroupToken);
                    }

                    var segemtToken = new IdocSegmentToken()
                    {
                        SegmentName = segement.Name,
                        Namespace = Namespace,
                        ChildTokens = segement.Fields.Select(f => new PropertyToken()
                        {
                            CSharpType = "object",
                            Description = f.Description,
                            PropertyName = f.Name
                        })
                    };

                    tokens.Add(segemtToken);
                }
            }

            return tokens;
        }

        private PropertyToken CreateSegmentProperty(IdocSegmentMetadata segmentMetadata)
        {
            if(segmentMetadata.IsGroup)
            {
                if (segmentMetadata.MaxGroupOccurrence > 1)
                    return new PropertyToken()
                        {
                            CSharpType = $"List<{segmentMetadata.Name}GRP>",
                            PropertyName = $"{segmentMetadata.Name}GRP",
                            Description = segmentMetadata.Description
                        };
                else
                    return new PropertyToken()
                    {
                        CSharpType = $"{segmentMetadata.Name}GRP",
                        PropertyName = $"{segmentMetadata.Name}GRP",
                        Description = segmentMetadata.Description
                    };
            }
            else
            {
                if (segmentMetadata.MaxOccurrence > 1)
                    return new PropertyToken()
                    {
                        CSharpType = $"List<{segmentMetadata.Name}>",
                        PropertyName = $"{segmentMetadata.Name}",
                        Description = segmentMetadata.Description
                    };
                else
                    return new PropertyToken()
                    {
                        CSharpType = $"{segmentMetadata.Name}",
                        PropertyName = $"{segmentMetadata.Name}",
                        Description = segmentMetadata.Description
                    };
            }
        }
    }
}
