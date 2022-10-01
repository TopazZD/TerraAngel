﻿using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using TerraAngel.WorldEdits;

namespace TerraAngel.Client.ClientWindows
{
    public class MainWindow : ClientWindow
    {
        public override Keys ToggleKey => ClientConfig.Settings.ToggleUIVisibility;

        public override bool DefaultEnabled => true;

        public override bool IsToggleable => true;

        public override string Title => "Main Window";
        public override bool IsPartOfGlobalUI => true;

        public override void Draw(ImGuiIOPtr io)
        {
            ImGui.PushFont(ClientAssets.GetMonospaceFont(16f));

            NVector2 windowSize = io.DisplaySize / new NVector2(3f, 2f);

            ImGui.SetNextWindowPos(new NVector2(0, io.DisplaySize.Y - windowSize.Y), ImGuiCond.FirstUseEver);
            ImGui.SetNextWindowSize(windowSize, ImGuiCond.FirstUseEver);

            ImGui.Begin("Main window");

            if (!Main.gameMenu && Main.CanUpdateGameplay)
            {
                DrawInGameWorld(io);
            }
            else
            {
                DrawInMenu(io);
            }


            ImGui.End();

            ImGui.PopFont();
        }

        public void DrawInGameWorld(ImGuiIOPtr io)
        {
            if (ImGui.BeginTabBar("##MainTabBar"))
            {
                if (ImGui.BeginTabItem("Cheats"))
                {
                    if (ImGui.BeginTabBar("CheatBar"))
                    {
                        if (ImGui.BeginTabItem("Main Cheats"))
                        {
                            foreach (Cringe cringe in CringeManager.GetCringeOfTab(CringeTabs.MainCringes))
                            {
                                cringe.DrawUI(io);
                            }
                            ImGui.EndTabItem();
                        }
                        if (ImGui.BeginTabItem("Butcher"))
                        {
                            if (ImGui.Button("Butcher All Hostile NPCs"))
                            {
                                Butcher.ButcherAllHostileNPCs(CringeManager.ButcherDamage);
                            }
                            ImGui.Checkbox("Auto-Butcher Hostiles", ref CringeManager.AutoButcherHostileNPCs);
                            if (ImGui.Button("Butcher All Friendly NPCs"))
                            {
                                Butcher.ButcherAllFriendlyNPCs(CringeManager.ButcherDamage);
                            }
                            if (ImGui.Button("Butcher All Players"))
                            {
                                Butcher.ButcherAllPlayers(CringeManager.ButcherDamage);
                            }
                            ImGui.SliderInt("Butcher Damage", ref CringeManager.ButcherDamage, 1, (int)short.MaxValue);

                            ImGui.EndTabItem();
                        }
                        if (ImGui.BeginTabItem("Items"))
                        {
                            if (ImGui.BeginTabBar("ItemBar"))
                            {
                                if (ImGui.BeginTabItem("Item Browser"))
                                {
                                    ItemBrowser.DrawBrowser();
                                    ImGui.EndTabItem();
                                }
                                ImGui.EndTabBar();
                            }
                            ImGui.EndTabItem();
                        }
                        if (ImGui.BeginTabItem("Automation"))
                        {
                            foreach (Cringe cringe in CringeManager.GetCringeOfTab(CringeTabs.AutomationCringes))
                            {
                                cringe.DrawUI(io);
                            }
                            ImGui.EndTabItem();
                        }
                        ImGui.EndTabBar();
                    }
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Visuals"))
                {
                    if (ImGui.BeginTabBar("VisualBar"))
                    {
                        if (ImGui.BeginTabItem("Utility"))
                        {
                            foreach (Cringe cringe in CringeManager.GetCringeOfTab(CringeTabs.VisualUtility))
                            {
                                cringe.DrawUI(io);
                            }

                            if (ImGui.Button("Reveal Map"))
                            {
                                Task.Run(() =>
                                {
                                    Stopwatch watch = Stopwatch.StartNew();
                                    int xlen = Main.Map.MaxWidth;
                                    int ylen = Main.Map.MaxHeight;
                                    for (int x = 0; x < xlen; x++)
                                    {
                                        for (int y = 0; y < ylen; y++)
                                        {
                                            if (Main.netMode == 0 || CringeManager.LoadedTileSections[x / 200, y / 150])
                                            {
                                                Main.Map.Update(x, y, 255);
                                            }
                                        }
                                    }
                                    watch.Stop();
                                    ClientLoader.Console.WriteLine($"Map took {watch.Elapsed.Milliseconds}ms");
                                    Main.refreshMap = true;
                                });
                            }
                            ImGui.EndTabItem();
                        }
                        if (ImGui.BeginTabItem("ESP"))
                        {
                            foreach (Cringe cringe in CringeManager.GetCringeOfTab(CringeTabs.ESP))
                            {
                                cringe.DrawUI(io);
                            }

                            ImGui.EndTabItem();
                        }
                        if (ImGui.BeginTabItem("Lighting"))
                        {
                            foreach (Cringe cringe in CringeManager.GetCringeOfTab(CringeTabs.LightingCringes))
                            {
                                cringe.DrawUI(io);
                            }
                            ImGui.EndTabItem();
                        }
                        ImGui.EndTabBar();
                    }
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("World Edit"))
                {
                    if (ImGui.BeginTabBar("WorldEditBar"))
                    {
                        for (int i = 0; i < ClientLoader.MainRenderer.WorldEdits.Count; i++)
                        {
                            WorldEdit worldEdit = ClientLoader.MainRenderer.WorldEdits[i];
                            if (worldEdit.DrawUITab(io))
                            {
                                ClientLoader.MainRenderer.CurrentWorldEditIndex = i;
                            }
                        }
                        ImGui.EndTabBar();
                    }
                    ImGui.EndTabItem();
                }
                else
                {
                    ClientLoader.MainRenderer.CurrentWorldEditIndex = -1;
                }

                if (ImGui.BeginTabItem("Misc"))
                {
                    if (ImGui.Checkbox("Nebula Spam", ref CringeManager.NebulaSpam))
                    {
                        if (CringeManager.NebulaSpam && CringeManager.NebulaSpamPower > 30)
                        {
                            CringeManager.GetCringe<Cheat.Cringes.OptimizationCringe>().DisableGore = true;
                        }
                    }
                    if (ImGui.CollapsingHeader("Nebula Settings"))
                    {
                        if (ImGui.SliderInt("Nebula Spam Power", ref CringeManager.NebulaSpamPower, 1, 500))
                        {
                            if (CringeManager.NebulaSpam && CringeManager.NebulaSpamPower > 30)
                            {
                                CringeManager.GetCringe<Cheat.Cringes.OptimizationCringe>().DisableGore = true;
                            }
                        }
                    }

                    ImGui.EndTabItem();
                }
                ImGui.EndTabBar();
            }
        }

        private int framesToShowUUIDFor = 0;
        public void DrawInMenu(ImGuiIOPtr io)
        {
            if (ImGui.BeginTabBar("##MainTabBar"))
            {
                if (ImGui.BeginTabItem("Cheats"))
                {
                    ImGui.Button($"{Icon.Refresh} Client UUID"); ImGui.SameLine();
                    if (ImGui.Button("Click to reveal"))
                    {
                        framesToShowUUIDFor = 600;
                    }
                    ImGui.SameLine();
                    if (ImGui.Button("Click to copy"))
                    {
                        ImGui.SetClipboardText(Main.clientUUID);
                    }

                    if (framesToShowUUIDFor > 0)
                    {
                        framesToShowUUIDFor--;
                        ImGui.Text(Main.clientUUID);
                    }

                    ImGui.EndTabItem();
                }

                ImGui.EndTabBar();
            }
        }

        public override void Update()
        {
            base.Update();

            // Optimization since WorldGen.TileFrame is called so fucking often
            Hooks.Hooks.MiscHooks.framingDisabled = CringeManager.GetCringe<DisableFramingCringe>().Enabled;
        }
    }
}
