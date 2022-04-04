using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SimpleFileBrowser;

public class AccessibleInteractable<T>: UAP_BaseElement where T: class, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

	//////////////////////////////////////////////////////////////////////////

	protected AccessibleInteractable()
	{
		m_Type = AccessibleUIGroupRoot.EUIElement.EButton;
	}

	//////////////////////////////////////////////////////////////////////////

	protected override void OnInteract()
	{
		// Press button (works for UGUI and TMP)
		T clicker = GetItem();
		if (clicker != null)
		{
			var pointer = new PointerEventData(EventSystem.current); // pointer event for Execute
			clicker.OnPointerClick(pointer);
			return;
		}
	}

	//////////////////////////////////////////////////////////////////////////

	public override bool IsElementActive()
	{
		// Return whether this button is usable, and visible
		if (!base.IsElementActive())
			return false;

		if (m_ReferenceElement != null)
			if (!m_ReferenceElement.gameObject.activeInHierarchy)
				return false;

		if (!UAP_AccessibilityManager.GetSpeakDisabledInteractables())
			if (!IsInteractable())
				return false;

		return true;
	}

	//////////////////////////////////////////////////////////////////////////

	private T GetItem()
	{
		T refClicker = null;
		if (m_ReferenceElement != null && m_ReferenceElement.activeInHierarchy)
			refClicker = m_ReferenceElement.GetComponent<T>();
		if (refClicker == null && gameObject.activeInHierarchy)
			refClicker = gameObject.GetComponent<T>();

		return refClicker;
	}

	public override bool IsInteractable()
	{
		T buttonComponent = GetItem();
		if (buttonComponent != null)
		{
			return true;
		}

		// We couldn't find any interactables...
		return false;
	}

	//////////////////////////////////////////////////////////////////////////

	public override bool AutoFillTextLabel()
	{
		if (base.AutoFillTextLabel())
			return true;

		bool found = false;

		// Unity UI
		//////////////////////////////////////////////////////////////////////////
		{
			// Try to find a label in the button's children
			Transform textLabel = gameObject.transform.Find("Text");
			if (textLabel != null)
			{
				Text label = textLabel.gameObject.GetComponent<Text>();
				if (label != null)
				{
					m_Text = label.text;
					found = true;
				}
			}

			if (!found)
			{
				Text label = gameObject.GetComponentInChildren<Text>();
				if (label != null)
				{
					m_Text = label.text;
					found = true;
				}
			}
		}

		// TextMesh Pro
		//////////////////////////////////////////////////////////////////////////
		if (!found)
		{
			var TMP_Label = GetTextMeshProLabelInChildren();
			if (TMP_Label != null)
			{
				m_Text = GetTextFromTextMeshPro(TMP_Label);
				found = true;
			}
		}

		// if nothing, use the GameObject name
		if (!found)
			m_Text = gameObject.name;

		return found;
	}

	//////////////////////////////////////////////////////////////////////////

	protected override void AutoInitialize()
	{
		if (m_TryToReadLabel)
		{
			bool found = false;

			// Unity UI
			//////////////////////////////////////////////////////////////////////////
			{
				// Try to find a label in the button's children
				Transform textLabel = gameObject.transform.Find("Text");
				if (textLabel != null)
				{
					Text label = textLabel.gameObject.GetComponent<Text>();
					if (label != null)
					{
						m_NameLabel = label.gameObject;
						found = true;
					}
				}

				if (!found)
				{
					Text label = gameObject.GetComponentInChildren<Text>();
					if (label != null)
					{
						m_NameLabel = label.gameObject;
						found = true;
					}
				}
			}

			// TextMesh Pro
			//////////////////////////////////////////////////////////////////////////
			if (!found)
			{
				var TMP_Label = GetTextMeshProLabelInChildren();
				if (TMP_Label != null)
				{
					m_NameLabel = TMP_Label.gameObject;
					found = true;
				}
			}
		}
		else
		{
			m_NameLabel = null;
		}
	}

	//////////////////////////////////////////////////////////////////////////

	protected override void OnHoverHighlight(bool enable)
	{
		T button = GetItem();
		if (button != null)
		{
			var pointer = new PointerEventData(EventSystem.current); // pointer event for Execute
			if (enable)
				button.OnPointerEnter(pointer);
			else
				button.OnPointerExit(pointer);
		}
	}

	//////////////////////////////////////////////////////////////////////////

}


