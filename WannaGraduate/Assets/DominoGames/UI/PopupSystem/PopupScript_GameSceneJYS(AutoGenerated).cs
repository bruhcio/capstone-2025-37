using System;

public enum EPopupNames_GameSceneJYS
{
    AddSymbol_Popup,
    ResearchResult_Popup,
}

namespace DominoGames.UI.PopupSystem
{
    public partial class PopupSystem{
        public class GameSceneJYS{
            public class AddSymbol_Popup{
                public static void Show(System.Object args, Action<PopupBase> direction = null)
                {
                    PopupSystem.Show(EPopupNames_GameSceneJYS.AddSymbol_Popup, args, direction);
                }
                public static void Hide(Action<PopupBase> direction = null)
                {
                    PopupSystem.Hide(EPopupNames_GameSceneJYS.AddSymbol_Popup, direction);
                }
            }
            public class ResearchResult_Popup{
                public static void Show(System.Object args, Action<PopupBase> direction = null)
                {
                    PopupSystem.Show(EPopupNames_GameSceneJYS.ResearchResult_Popup, args, direction);
                }
                public static void Hide(Action<PopupBase> direction = null)
                {
                    PopupSystem.Hide(EPopupNames_GameSceneJYS.ResearchResult_Popup, direction);
                }
            }
        }
    }
}
