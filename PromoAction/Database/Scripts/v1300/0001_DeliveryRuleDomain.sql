update promo.RuleDomains
set Metadata.modify('delete /EntitiesMetadata/Entities[EntityName/text()="p"]/Attributes[Name/text()="CategoryId" or Name/text()="ProductId"]')
where Id = 2
