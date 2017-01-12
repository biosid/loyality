using System.Xml;
using System.Data;
using System.Collections;

namespace Common
{
	/// <summary>
	/// Summary description for XML.
	/// </summary>
	public class XML
	{
		public XML()
		{ 

		}

		public static bool IsXml(object p_sXml)
		{
			if(p_sXml == null || p_sXml.ToString () == "" || p_sXml.ToString ().Length < 7) // <a></a> - shortest xml
				return false;

            string sXml = p_sXml.ToString();
            //No tags in expression
            if (sXml.IndexOf('<') < 0 || sXml.IndexOf('>') < 0)
                return false;

            if (char.IsDigit(sXml[0]))
                return false;

			try
			{
				XmlDocument xd = new XmlDocument();
                xd.LoadXml(sXml);
				return true;
			}catch{
				return false;
			}
		}

		public static string GetNodeAttribute(XmlNode p_Node, string p_sAttribute)
		{
			string sAttributeValue;

            if (p_Node == null || p_Node.Attributes[p_sAttribute] == null)
                return "";

			try
			{
				sAttributeValue = p_Node.Attributes[p_sAttribute].Value;
				sAttributeValue = XmlConvert.DecodeName(sAttributeValue); 
			}
			catch
			{                
				sAttributeValue = "";
			}
			return sAttributeValue;
		}

		public static void SetNodeAttribute(XmlNode p_Node, string p_sAttribute, string p_sAttributeValue)
		{
			XmlAttribute attribute = p_Node.Attributes[p_sAttribute];

			if (attribute == null)
			{
				attribute = p_Node.OwnerDocument.CreateAttribute(p_sAttribute);
				p_Node.Attributes.Append(attribute);
			}
			attribute.Value = p_sAttributeValue;
		}

		public static XmlNode GetElement(XmlNode p_Node, string p_sName)
		{
			XmlNode node = p_Node.SelectSingleNode(p_sName);
			
			if (node == null)
				node = CreateElement(p_Node, p_sName);

			p_Node.AppendChild(node);

			return node;
		}

		public static XmlNode GetElement(XmlNode p_Node, string p_sName, string p_sAttribute, string p_sAttributeValue)
		{
			string sXPath = p_sName + "[@" + p_sAttribute + "='" + p_sAttributeValue + "']";
			XmlNode node = p_Node.SelectSingleNode(sXPath);

			if (node == null)
				node = CreateElement(p_Node, p_sName);

			SetNodeAttribute(node, p_sAttribute, p_sAttributeValue);

			p_Node.AppendChild(node);

			return node;
		}

		public static XmlElement CreateElement(XmlNode p_Node, string p_sName, string p_sInnerText)
		{
			XmlDocument xmlDoc;
			if (p_Node is XmlDocument)
				xmlDoc = (XmlDocument)p_Node;
			else
				xmlDoc =p_Node.OwnerDocument;

			XmlElement element = xmlDoc.CreateElement(p_sName);
			element.InnerText = p_sInnerText;		
			return element;
		}

		public static XmlElement AppendElement(XmlNode p_Node, string p_sName, string p_sInnerText)
		{
			XmlElement element = CreateElement(p_Node,  p_sName, p_sInnerText);
			p_Node.AppendChild(element);
			return element;
		}

		public static XmlElement AppendElement(XmlNode p_Node, string p_sName)
		{
			return AppendElement(p_Node,  p_sName, "");
		}

		public static XmlElement CreateElement(XmlNode p_Node, string p_sName)
		{
			return CreateElement(p_Node, p_sName, "");
		}


		public static XmlDocument CreateXmlDocumentWithRoot(string p_sRoot)
		{
			XmlDocument xd = new XmlDocument ();
			XmlNode xn = xd.CreateNode (XmlNodeType.Element, p_sRoot, "");
			xd.AppendChild(xn);
			return xd;		
		}


		public static XmlDocument GetXmlDocument(XmlNode p_xn)
		{	
			XmlDocument xd;		
			if (p_xn is XmlDocument)
				xd = (XmlDocument)p_xn;
			else
				xd = p_xn.OwnerDocument;
			return xd;
		}


		public static XmlNode CreateXmlNode(XmlNode p_Node, string p_sName, string p_sInnerXml)
		{
			XmlDocument xmlDoc = GetXmlDocument(p_Node);			
			XmlNode xn = xmlDoc.CreateNode (XmlNodeType.Element, p_sName, "");
			xn.InnerXml = p_sInnerXml;		
			return xn;
		}

		public static XmlNode CreateXmlNode(XmlNode p_Node, string p_sName, string p_sUri, string p_sInnerXml)
		{
			XmlDocument xmlDoc = GetXmlDocument(p_Node);			
			XmlNode xn = xmlDoc.CreateNode (XmlNodeType.Element, p_sName, p_sUri);
			xn.InnerXml = p_sInnerXml;		
			return xn;
		}

		public static XmlNode AppendXmlNode(XmlNode p_Node, string p_sName, string p_sInnerXml)
		{
			XmlNode xn = CreateXmlNode(p_Node,  p_sName, p_sInnerXml);
			p_Node.AppendChild(xn);
			return xn;
		}

		public static XmlNode AppendCopyOfNode(XmlNode p_parentNode, XmlNode p_Node)
		{
			XmlNode node = AppendXmlNode(p_parentNode, p_Node.Name, p_Node.InnerXml);
			CopyAllAttributes(p_Node, node);
			return node;
		}
		

		
		public static XmlNode AppendXmlNodeText(XmlNode p_Node, string p_sName, string p_sInnerText)
		{
			XmlNode xn = CreateXmlNode(p_Node,  p_sName, "");
			xn.InnerText = p_sInnerText;
			p_Node.AppendChild(xn);
			return xn;
		}

		public static XmlNode AppendXmlNode(XmlNode p_Node, string p_sName, string p_sUri, string p_sInnerText)
		{
			XmlNode xn = CreateXmlNode(p_Node,  p_sName, p_sUri,  p_sInnerText);
			p_Node.AppendChild(xn);
			return xn;
		}

		public static XmlNode AppendAttribute(XmlNode p_Node, string p_sName, string p_sValue)
		{
			XmlAttribute xa = CreateXmlAttribute(p_Node,  p_sName, p_sValue);
			p_Node.Attributes.Append(xa);
			return xa;
		}
		
		public static XmlAttribute CreateXmlAttribute(XmlNode p_Node, string p_sName, string p_sValue)
		{
			XmlDocument xd = GetXmlDocument(p_Node);			
			XmlAttribute xa = xd.CreateAttribute (p_sName);
			xa.InnerText = p_sValue;
			return xa;		
		}

		public static void ConvertChildsNodesToAttributes(XmlNode p_xn)
		{
			for(int i = 0; i < p_xn.ChildNodes.Count; i++)
				ConvertNodesToAttributes(p_xn.ChildNodes[i]);
		}
		public static void ConvertNodesToAttributes(XmlNode p_xn)
		{
			int iCount = p_xn.ChildNodes.Count;
			for(int i = 0; i < iCount; i++)
			{
				XmlNode xn = p_xn.ChildNodes[0];
				XmlAttribute xa = CreateXmlAttribute (p_xn, xn.Name, xn.InnerText);
				p_xn.Attributes.Append (xa);
				p_xn.RemoveChild(xn);
			}

		}


		public static XmlDocument ConvertDataTableToXmlDocument(DataTable p_dt, string p_sRootNodeName, string p_sRecordNodeName, Hashtable p_htReplaceColumnNames)
		{
			XmlDocument xd = CreateXmlDocumentWithRoot (p_sRootNodeName);
			XmlNode xnRoot = xd.ChildNodes[0];
			for(int r = 0; r < p_dt.Rows.Count; r++)
			{
				XmlNode xnRecord = AppendXmlNode (xnRoot, p_sRecordNodeName, "");
				for(int c = 0; c < p_dt.Columns.Count; c++)				
				{
					string sColumnName = p_dt.Columns[c].ColumnName;
					if(p_htReplaceColumnNames != null)
					{
						string sNewColumnName = (string)(p_htReplaceColumnNames[sColumnName]);
						if(sNewColumnName != null)
							sColumnName = sNewColumnName;
					}

					AppendXmlNodeText(xnRecord, sColumnName, p_dt.Rows[r][c].ToString ()); 															
				}
			}

			return xd;
		
		}

		public static void DeleteChildNodesByName(XmlNode p_node, string p_sNodesName)
		{
			XmlNodeList nodes = p_node.SelectNodes(p_sNodesName);
			foreach(XmlNode nodeChild in nodes)
			{
				p_node.RemoveChild(nodeChild);
			}
		}

#if FRAMEWORK_2

        public static Unit? GetUnit(XmlNode xnElement, string xpath)
        {
            try
            {
                XmlNode oNode = xnElement.SelectSingleNode(xpath);

                if (oNode != null)
                    return Unit.Parse(oNode.Value);

                return null;
            }
            catch
            {
                return null;
            }
        }

        public static int? GetInt(XmlNode xnElement, string xpath)
        {
            try
            {
                XmlNode oNode = xnElement.SelectSingleNode(xpath);

                if (oNode != null)
                    return int.Parse(oNode.Value);

                return null;
            }
            catch
            {
                return null;
            }
        }

        public static Unit? GetUnitAttribute(XmlNode xnElement, string attributeName)
        {
            return GetUnit(xnElement, "@" + attributeName);
        }

        public static int? GetIntAttribute(XmlNode xnElement, string attributeName)
        {
            return GetInt(xnElement, "@" + attributeName);
        }

        public static string GetInnerText(XmlNode xnElement)
        {
            try
            {
                return xnElement.InnerText;
            }
            catch
            {
                return null;
            }
        }


#endif

		public static XmlDocument LoadDocument(string xml)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			return xmlDocument;
		}
		
		public static void CopyAttribute(XmlNode p_nodeFrom, XmlNode p_nodeTo, string p_sAttributeName)
		{
			SetNodeAttribute(p_nodeTo, p_sAttributeName, GetNodeAttribute(p_nodeFrom, p_sAttributeName));
		}
		
		public static void CopyAttribute(XmlNode p_nodeFrom, XmlNode p_nodeTo, string p_sAttributeName, bool p_bIgnoreNull)
		{
			string sValue = GetNodeAttribute(p_nodeFrom, p_sAttributeName);
			if (!p_bIgnoreNull || sValue.Length > 0)
				SetNodeAttribute(p_nodeTo, p_sAttributeName, sValue);
		}
		
		public static void CopyAllAttributes(XmlNode p_nodeFrom, XmlNode p_nodeTo)
		{
			for(int i = 0; i < p_nodeFrom.Attributes.Count; i++)
			{
				CopyAttribute(p_nodeFrom, p_nodeTo, p_nodeFrom.Attributes[i].Name);
			}
		}

        public static XmlNode GetNextElement(XmlNode p_xnElement)
        {
            if (p_xnElement == null)
                return null;
            p_xnElement = p_xnElement.NextSibling;
            while (p_xnElement != null && p_xnElement.NodeType != XmlNodeType.Element)
                p_xnElement = p_xnElement.NextSibling;

            return p_xnElement;
        }

        public static XmlNode GetPreviousElement(XmlNode p_xnElement)
        {
            if (p_xnElement == null)
                return null;
            p_xnElement = p_xnElement.PreviousSibling;
            while (p_xnElement != null && p_xnElement.NodeType != XmlNodeType.Element)
                p_xnElement = p_xnElement.PreviousSibling;

            return p_xnElement;
        }

	    public static XmlNode GetElementByIndex(XmlNode p_oParentNode, string p_sElementName, int p_iElementIndex)
	    {
            if (p_iElementIndex >= 0 && p_iElementIndex < p_oParentNode.ChildNodes.Count)
            {
                foreach (XmlNode xnNode in p_oParentNode.ChildNodes)
                {
                    if (xnNode.NodeType != XmlNodeType.Element || (p_sElementName != null && xnNode.Name != p_sElementName))
                        continue;

                    if (p_iElementIndex == 0)
                        return xnNode;

                    p_iElementIndex--;
                }
            }
	        return null;
	    }
	}
}
