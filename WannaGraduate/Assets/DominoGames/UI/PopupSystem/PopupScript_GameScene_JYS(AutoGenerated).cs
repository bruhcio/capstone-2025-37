using System;

public enum EPopupNames_GameScene_JYS
{
    AddSymbol_Popup,
}

namespace DominoGames.UI.PopupSystem
{
    public partial class PopupSystem{
        public class GameScene_JYS{
            public class AddSymbol_Popup{
                public static void Show(System.Object args, Action<PopupBase> direction = null)
                {
                    PopupSystem.Show(EPopupNames_GameScene_JYS.AddSymbol_Popup, args, direction);
                }
                public static void Hide(Action<PopupBase> direction = null)
                {
                    PopupSystem.Hide(EPopupNames_GameScene_JYS.AddSymbol_Popup, direction);
                }
            }
        }
    }
}
