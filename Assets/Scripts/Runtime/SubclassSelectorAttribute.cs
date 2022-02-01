using System;
using UnityEngine;

/// <summary>
/// SerializeReferenceの項目を表示してくれるEditor拡張用クラス
/// 
/// NOTE: とくに触る必要のないコード
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class SubclassSelectorAttribute : PropertyAttribute
{
	bool m_includeMono;

	public SubclassSelectorAttribute(bool includeMono = false)
	{
		m_includeMono = includeMono;
	}

	public bool IsIncludeMono()
	{
		return m_includeMono;
	}
}