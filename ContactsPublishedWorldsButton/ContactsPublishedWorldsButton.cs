using FrooxEngine;
using FrooxEngine.UIX;
using CloudX.Shared;
using HarmonyLib;
using NeosModLoader;

namespace ContactsPublishedWorldsButton
{
    public class ContactsPublishedWorldsButton : NeosMod
    {
        public override string Name => "ContactsPublishedWorldsButton";
        public override string Author => "badhaloninja";
        public override string Version => "1.0.0";
        public override string Link => "https://github.com/badhaloninja/ContactsPublishedWorldsButton";
        public override void OnEngineInit()
        {
            Harmony harmony = new Harmony("me.badhaloninja.ContactsPublishedWorldsButton");
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(FriendsDialog), "UpdateSelectedFriend")]
        class InventoryBrowser_OnItemSelected_Patch
        {
            public static void Postfix(FriendsDialog __instance, UIBuilder ___actionsUi)
            {
                if (__instance.SelectedFriend == null || __instance.SelectedFriend.FriendUserId == "U-Neos") return;
                UIBuilder actionsUIBuilder = ___actionsUi;
                actionsUIBuilder.Button("Show Worlds").LocalPressed += (IButton button, ButtonEventData eventData) =>
                {
                    setWorldScreenValues(__instance);
                };
            }
        }

        public static void setWorldScreenValues(FriendsDialog friendsDialog)
        {
            RadiantDash dash = friendsDialog.Slot.GetComponentInParents<RadiantDash>();
            GridContainerScreen WorldsScreen = dash.GetScreen<GridContainerScreen>((GridContainerScreen g) => g.HasPreset(typeof(WorldsScreenInitializer)));
            
            DynamicVariableSpace space = WorldsScreen.Slot.FindSpace("");
            space.TryWriteValue<string>("Worlds.HiddenSearchTerm", "");
            space.TryWriteValue<string>("Worlds.ByOwner", friendsDialog.SelectedFriend.FriendUserId);
            space.TryWriteValue<OwnerType>("Worlds.OwnerType", OwnerType.User);
            space.TryWriteValue<bool>("Worlds.ShowOpenedWorlds", true);
            space.TryWriteValue<bool>("Worlds.ShowSessions", true);
            space.TryWriteValue<bool>("Worlds.ShowPublishedWorlds", true);
            space.TryWriteValue<bool>("Worlds.ShowLocalWorlds", true);
            space.TryWriteValue<bool>("Worlds.OnlyFeatured", false);

            dash.CurrentScreen.Target = WorldsScreen;
        }
    }
}