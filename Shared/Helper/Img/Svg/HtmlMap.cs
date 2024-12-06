﻿namespace Shared.Helper.Img.Svg
{
    public class HtmlMap
    {
        public static string Orion => "<svg xmlns=\"http://www.w3.org/2000/svg\" height=\"24px\" viewBox=\"0 0 24 24\" width=\"24px\" fill=\"#0000F5\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M2 12C2 6.48 6.48 2 12 2s10 4.48 10 10-4.48 10-10 10S2 17.52 2 12zm10 6c3.31 0 6-2.69 6-6s-2.69-6-6-6-6 2.69-6 6 2.69 6 6 6z\"/></svg>";

        public static string Navigation => "<svg xmlns=\"http://www.w3.org/2000/svg\" height=\"24px\" viewBox=\"0 -960 960 960\" width=\"24px\" fill=\"#000000\"><path d=\"m200-120-40-40 320-720 320 720-40 40-280-120-280 120Zm84-124 196-84 196 84-196-440-196 440Zm196-84Z\"/></svg>";
        public static string NavigationRotation(int rot)
        {
            return "<svg xmlns=\"http://www.w3.org/2000/svg\" height =\"24px\" viewBox =\"0 -960 960 960\" width =\"24px\" fill=\"#0000F5\" transform=\"rotate(" + rot + ")\"><path d=\"m319-280 161-73 161 73 15-15-176-425-176 425 15 15ZM480-80q-83 0-156-31.5T197-197q-54-54-85.5-127T80-480q0-83 31.5-156T197-763q54-54 127-85.5T480-880q83 0 156 31.5T763-763q54 54 85.5 127T880-480q0 83-31.5 156T763-197q-54 54-127 85.5T480-80Zm0-80q134 0 227-93t93-227q0-134-93-227t-227-93q-134 0-227 93t-93 227q0 134 93 227t227 93Zm0-320Z\"/></svg>";
        }
        public static string AssistantNavigation => "<svg xmlns=\"http://www.w3.org/2000/svg\" height=\"24px\" viewBox=\"0 -960 960 960\" width=\"24px\" fill=\"#000000\"><path d=\"m319-280 161-73 161 73 15-15-176-425-176 425 15 15ZM480-80q-83 0-156-31.5T197-197q-54-54-85.5-127T80-480q0-83 31.5-156T197-763q54-54 127-85.5T480-880q83 0 156 31.5T763-763q54 54 85.5 127T880-480q0 83-31.5 156T763-197q-54 54-127 85.5T480-80Zm0-80q134 0 227-93t93-227q0-134-93-227t-227-93q-134 0-227 93t-93 227q0 134 93 227t227 93Zm0-320Z\"/></svg>";

        public static string AssistanceDirectionDark => "<svg xmlns=\"http://www.w3.org/2000/svg\" height=\"24px\" viewBox=\"0 -960 960 960\" width=\"24px\" fill=\"#000000\"><path d=\"M480-40q-91 0-171.5-34.5t-140-94q-59.5-59.5-94-140T40-480q0-92 34.5-172t94-139.5q59.5-59.5 140-94T480-920q92 0 172 34.5t139.5 94Q851-732 885.5-652T920-480q0 91-34.5 171.5t-94 140Q732-109 652-74.5T480-40Zm0-440Zm-25 311q10 10 23 10t23-10l288-288q10-10 10-24t-10-24L501-793q-10-10-23-10t-23 10L167-505q-10 10-10 24t10 24l288 288ZM319-361v-160q0-18 11-29t29-11h166l-42-44 56-56 140 140-140 140-56-56 42-44H399v120h-80Zm161 241q151 0 255.5-104.5T840-480q0-151-104.5-255.5T480-840q-151 0-255.5 104.5T120-480q0 151 104.5 255.5T480-120Z\"/></svg>";
        public static string MapDark => "<svg xmlns=\"http://www.w3.org/2000/svg\" height=\"24px\" viewBox=\"0 -960 960 960\" width=\"24px\" fill=\"#000000\"><path d=\"m600-120-240-84-186 72q-20 8-37-4.5T120-170v-560q0-13 7.5-23t20.5-15l212-72 240 84 186-72q20-8 37 4.5t17 33.5v560q0 13-7.5 23T812-192l-212 72Zm-40-98v-468l-160-56v468l160 56Zm80 0 120-40v-474l-120 46v468Zm-440-10 120-46v-468l-120 40v474Zm440-458v468-468Zm-320-56v468-468Z\"/></svg>";
        public static string AssistanceDirectionWhite => "<svg xmlns=\"http://www.w3.org/2000/svg\" height=\"24px\" viewBox=\"0 -960 960 960\" width=\"24px\" fill=\"#e8eaed\"><path d=\"M480-40q-91 0-171.5-34.5t-140-94q-59.5-59.5-94-140T40-480q0-92 34.5-172t94-139.5q59.5-59.5 140-94T480-920q92 0 172 34.5t139.5 94Q851-732 885.5-652T920-480q0 91-34.5 171.5t-94 140Q732-109 652-74.5T480-40Zm0-440Zm-25 311q10 10 23 10t23-10l288-288q10-10 10-24t-10-24L501-793q-10-10-23-10t-23 10L167-505q-10 10-10 24t10 24l288 288ZM319-361v-160q0-18 11-29t29-11h166l-42-44 56-56 140 140-140 140-56-56 42-44H399v120h-80Zm161 241q151 0 255.5-104.5T840-480q0-151-104.5-255.5T480-840q-151 0-255.5 104.5T120-480q0 151 104.5 255.5T480-120Z\"/></svg>";
        public static string MapWhite => "<svg xmlns=\"http://www.w3.org/2000/svg\" height=\"24px\" viewBox=\"0 -960 960 960\" width=\"24px\" fill=\"#e8eaed\"><path d=\"m600-120-240-84-186 72q-20 8-37-4.5T120-170v-560q0-13 7.5-23t20.5-15l212-72 240 84 186-72q20-8 37 4.5t17 33.5v560q0 13-7.5 23T812-192l-212 72Zm-40-98v-468l-160-56v468l160 56Zm80 0 120-40v-474l-120 46v468Zm-440-10 120-46v-468l-120 40v474Zm440-458v468-468Zm-320-56v468-468Z\"/></svg>";

        public static string MyLocationDark => "<svg xmlns=\"http://www.w3.org/2000/svg\" height=\"24px\" viewBox=\"0 -960 960 960\" width=\"24px\" fill=\"#000000\"><path d=\"M440-42v-80q-125-14-214.5-103.5T122-440H42v-80h80q14-125 103.5-214.5T440-838v-80h80v80q125 14 214.5 103.5T838-520h80v80h-80q-14 125-103.5 214.5T520-122v80h-80Zm40-158q116 0 198-82t82-198q0-116-82-198t-198-82q-116 0-198 82t-82 198q0 116 82 198t198 82Zm0-120q-66 0-113-47t-47-113q0-66 47-113t113-47q66 0 113 47t47 113q0 66-47 113t-113 47Zm0-80q33 0 56.5-23.5T560-480q0-33-23.5-56.5T480-560q-33 0-56.5 23.5T400-480q0 33 23.5 56.5T480-400Zm0-80Z\"/></svg>";
        public static string MyLocationWhite => "<svg xmlns=\"http://www.w3.org/2000/svg\" height=\"24px\" viewBox=\"0 -960 960 960\" width=\"24px\" fill=\"#e8eaed\"><path d=\"M440-42v-80q-125-14-214.5-103.5T122-440H42v-80h80q14-125 103.5-214.5T440-838v-80h80v80q125 14 214.5 103.5T838-520h80v80h-80q-14 125-103.5 214.5T520-122v80h-80Zm40-158q116 0 198-82t82-198q0-116-82-198t-198-82q-116 0-198 82t-82 198q0 116 82 198t198 82Zm0-120q-66 0-113-47t-47-113q0-66 47-113t113-47q66 0 113 47t47 113q0 66-47 113t-113 47Zm0-80q33 0 56.5-23.5T560-480q0-33-23.5-56.5T480-560q-33 0-56.5 23.5T400-480q0 33 23.5 56.5T480-400Zm0-80Z\"/></svg>";



    }
}