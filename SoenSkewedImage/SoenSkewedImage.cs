using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Soen
{

    public class SoenSkewedImage : Image
    {
        //이 클래스는 아래를 고친 것입니다 : 
        //originated from :
        //https://github.com/revolt3r/SkewedImage

        //아래 웹페이지에서 soengame의 최신 정보를 얻으세요!
        //Get the latest information on soengame from the web page below!
        //https://www.facebook.com/profile.php?id=100017292964953


        public Vector2 SkewVector;


        protected override void OnPopulateMesh(VertexHelper vh)
        {
            base.OnPopulateMesh(vh);

            Rect destRect = GetPixelAdjustedRect();

            Color32 color32 = color;

            vh.Clear();

            float xSprite=0.0f;
            float ySprite=0.0f;
            float xMaxSprite=0.0f;
            float yMaxSprite = 0.0f;
            Vector4 baseVector = Vector4.one;
            {
                if (sprite != null)
                {
                    switch (type)
                    {
                        case Type.Simple:
                            xSprite = sprite.rect.x;
                            ySprite = sprite.rect.y;
                            xMaxSprite = sprite.rect.xMax;
                            yMaxSprite = sprite.rect.yMax;
                            baseVector = new Vector4(destRect.x, destRect.y, destRect.x + destRect.width, destRect.y + destRect.height);
                            break;


                        case Type.Filled:
                            {
                                switch (fillMethod)
                                {
                                    case FillMethod.Horizontal:
                                        {
                                            xSprite = sprite.rect.x;
                                            ySprite = sprite.rect.y;

                                            float w = sprite.rect.width * fillAmount;
                                            xMaxSprite = xSprite + w;
                                            yMaxSprite = sprite.rect.yMax;

                                            float w2 = (destRect.width / sprite.rect.width) * w;
                                            baseVector = new Vector4(destRect.x, destRect.y, destRect.x + w2, destRect.y + destRect.height);

                                        }
                                        break;

                                    case FillMethod.Vertical:
                                        {
                                            xSprite = sprite.rect.x;
                                            ySprite = sprite.rect.y;

                                            float h = sprite.rect.height * fillAmount;
                                            xMaxSprite = sprite.rect.xMax;
                                            yMaxSprite = ySprite + h;

                                            float h2 = (destRect.height / sprite.rect.height) * h;
                                            baseVector = new Vector4(destRect.x, destRect.y, destRect.x + destRect.width, destRect.y + h2);

                                        }
                                        break;

                                    case FillMethod.Radial90:
                                    case FillMethod.Radial360:
                                    case FillMethod.Radial180:
                                    default:
                                        Debug.LogError("[SoenSkewedImage] not implemented yet :Radial90 / Radial370 / Radial180");
                                        break;
                                }
                            }

                            break;

                        case Type.Sliced:
                        case Type.Tiled:
                        default:
                            Debug.LogError("[SoenSkewedImage] not implemented yet : Type.Sliced, Type.Tiled.");
                            break;
                    }
                }
            }

            float wTexture;
            float hTexture;
            getTextureSizes(out wTexture, out hTexture);

            float xRatio = xSprite / wTexture;
            float yRatio = ySprite / hTexture;
            float xMaxRatio = xMaxSprite / wTexture;
            float yMaxRatio = yMaxSprite / hTexture;


            Vector3 destVert1 = new Vector3(baseVector.x - SkewVector.x, baseVector.y - SkewVector.y);
            Vector3 destVert2 = new Vector3(baseVector.x + SkewVector.x, baseVector.w - SkewVector.y);
            Vector3 destVert3 = new Vector3(baseVector.z + SkewVector.x, baseVector.w + SkewVector.y);
            Vector3 destVert4 = new Vector3(baseVector.z - SkewVector.x, baseVector.y + SkewVector.y);

            vh.AddVert(destVert1, color32, new Vector2(xRatio, yRatio));
            vh.AddVert(destVert2, color32, new Vector2(xRatio, yMaxRatio));
            vh.AddVert(destVert3, color32, new Vector2(xMaxRatio, yMaxRatio));
            vh.AddVert(destVert4, color32, new Vector2(xMaxRatio, yRatio));

            vh.AddTriangle(0, 1, 2);
            vh.AddTriangle(2, 3, 0);

        }

        private void getTextureSizes(out float wTexture, out float hTexture)
        {
            if (sprite == null)
            {
                wTexture = 0.0f;
                hTexture = 0.0f;
            }
            else
            {
                wTexture=sprite.texture.width;
                hTexture = sprite.texture.height;
            }
        }

    }

}

