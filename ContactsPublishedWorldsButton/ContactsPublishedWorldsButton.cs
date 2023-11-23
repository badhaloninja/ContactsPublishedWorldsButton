using FrooxEngine;
using FrooxEngine.UIX;
using SkyFrost.Base;
using HarmonyLib;
using ResoniteModLoader;

namespace ContactsPublishedWorldsButton
{
    public class ContactsPublishedWorldsButton : ResoniteMod
    {
        public override string Name => "ContactsPublishedWorldsButton";
        public override string Author => "badhaloninja, gameboycjp";
        public override string Version => "2.0.0";
        public override string Link => "https://github.com/badhaloninja/ContactsPublishedWorldsButton";
        public override void OnEngineInit()
        {
            Harmony harmony = new Harmony("gameboycjp.KarIO.ContactsPublishedWorldsButton");
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(ContactsDialog), "UpdateSelectedContact")]
        class InventoryBrowser_OnItemSelected_Patch
        {
            public static void Postfix(ContactsDialog __instance, UIBuilder ___actionsUi)
            {
                if (__instance.SelectedContact == null || __instance.SelectedContact.ContactUserId == "U-Resonite") return;
                UIBuilder actionsUIBuilder = ___actionsUi;
                actionsUIBuilder.Button("Show Worlds").LocalPressed += (IButton button, ButtonEventData eventData) =>
                {
                    setWorldScreenValues(__instance);
                };
            }
        }

        public static void setWorldScreenValues(ContactsDialog ContactsDialog)
        {
            RadiantDash dash = ContactsDialog.Slot.GetComponentInParents<RadiantDash>();
            GridContainerScreen WorldsScreen = dash.GetScreen<GridContainerScreen>((GridContainerScreen g) => g.HasPreset(typeof(WorldsScreenInitializer)));
            
            DynamicVariableSpace space = WorldsScreen.Slot.FindSpace("");
            space.TryWriteValue<string>("Worlds.HiddenSearchTerm", "");
            space.TryWriteValue<string>("Worlds.ByOwner", ContactsDialog.SelectedContact.ContactUserId);
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