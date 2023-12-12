# README - GhostOnly
본 레포지토리는 코드 공개를 위해 유료 에셋을 제거한 상태입니다. <br>
따라서 유니티에서 열면 에러가 뜨기 때문에 에러 없이 원본 레포지토리를 보고 싶으신 분은 아래 주소로 메일 남겨주시면 감사하겠습니다.<br>
**원본 레포지토리 권한 요청 메일 주소: she79560@gmail.com**

---
### 목차

1. [**게임 소개**](https://github.com/Dream-for-sever-cost/GhostOnly-Removed-paid-assets-#1-%EA%B2%8C%EC%9E%84-%EC%86%8C%EA%B0%9C)
2. [**팀원 소개 및 개발 기간**](https://github.com/Dream-for-sever-cost/GhostOnly-Removed-paid-assets-#2-%ED%8C%80%EC%9B%90-%EC%86%8C%EA%B0%9C-%EB%B0%8F-%EA%B0%9C%EB%B0%9C-%EA%B8%B0%EA%B0%84-)
3. [**기능별 클래스 설명**](https://github.com/Dream-for-sever-cost/GhostOnly-Removed-paid-assets-#3-%EA%B8%B0%EB%8A%A5%EB%B3%84-%ED%81%B4%EB%9E%98%EC%8A%A4-%EC%84%A4%EB%AA%85-)
4. [**사용한 기술 목록**](https://github.com/Dream-for-sever-cost/GhostOnly-Removed-paid-assets-#4-%EC%82%AC%EC%9A%A9%ED%95%9C-%EA%B8%B0%EC%88%A0-%EB%AA%A9%EB%A1%9D-)
5. [**라이선스**](https://github.com/Dream-for-sever-cost/GhostOnly-Removed-paid-assets-#6-%EB%9D%BC%EC%9D%B4%EC%84%A0%EC%8A%A4-)

---

# 1. 게임 소개

### 게임명: GhostOnly
**🎥이미지를 누르면 홍보 영상으로 이동합니다.🎥**
[![Video Label](https://github.com/jungbosong/GhostOnly/blob/YJY_README/ReadMe/main.png)](https://www.youtube.com/watch?v=80ByDIkzFlA)

### 장르 : 실시간 전략 디펜스 (싱글 플레이)

플레이어는 유령이 되어 해골을 조종해 영웅을 물리치며, 제단에 영혼을 바쳐 고대 악마님을 부활시켜야 하는 게임입니다.
*자세한 설명은 게임 설명서 참고*

### 스토리 [🔝](https://github.com/Dream-for-sever-cost/GhostOnly-Removed-paid-assets-#%EB%AA%A9%EC%B0%A8)

```
평화로운 나날.
나는 오늘도 고대 악마를 부활시키기 위해 영혼을 수집하고 있다.
하지만 영웅들이 나를 가만히 둘리가 없다.
해골들과 함께 방해하는 영웅들을 제쳐두고 고대 악마님을 부활시켜 혼돈을 불러오자 !
```

### 프로젝트 목표 [🔝](https://github.com/Dream-for-sever-cost/GhostOnly-Removed-paid-assets-#%EB%AA%A9%EC%B0%A8)

- **스팀 출시**
    - **개발 기간**: `2023.10.23 ~ 2023.12.15`

### 프로젝트 개요 [🔝](https://github.com/Dream-for-sever-cost/GhostOnly-Removed-paid-assets-#%EB%AA%A9%EC%B0%A8)

***보다 자세한 내용은 게임 설명서를 참고해 주세요.***

---

# 2. 팀원 소개 및 개발 기간 [🔝](https://github.com/Dream-for-sever-cost/GhostOnly-Removed-paid-assets-#%EB%AA%A9%EC%B0%A8)

## 2-1.  팀원 소개

| 팀원명 | 담당 기능 | GitHub | Blog |
| --- | --- | --- | --- |
|윤지연(리더)| 게임UI, 국제화, 버전 관리, 데이터 암호화, 시간 흐름, 마도서, 관:상점 | https://github.com/jungbosong | https://while0night.tistory.com/ |
|김민상(부리더)| 애니메이션, 디자인, 게임UI, 시간흐름, 상호작용, 제단, 유령(플레이어), 해골, 영웅-침입, 장비, 게임 목표(로직), InputAciton | https://github.com/berylstar | https://velog.io/@berylstar |
|강성호| 인트로, 게임 결과창, 사운드, 묘비: 영혼 수확, 장비, 미니맵, DOTween, 게임 목표(로직) | https://github.com/tjdgh7419 | https://tjdgh7419.tistory.com/ |
|김대열| 로비, 설정, 국제화, 버전 관리, 시간 흐름, 사운드, 해골, 해골-특성, 해골-자동화, 해골-상태창 | https://github.com/Kim-dae-yeol?tab=repositories | https://velog.io/@elmo7180 |
|조범준| 로비, 설정, 튜토리얼, 사운드, 영웅, 유령의 영지, DOTween, NavMesh | https://github.com/KimMaYa1 | https://beomjun-develop.tistory.com/ |

## 2-2. 개발 기간 [🔝](https://github.com/Dream-for-sever-cost/GhostOnly-Removed-paid-assets-#%EB%AA%A9%EC%B0%A8)

| 기간 | 작업 목록 |
| --- | --- |
| 1W 2023.10.23 ~ 2023.10.29 | • 기획<br> • 필요한 기능 정리 및 역할 분담<br> • 프로토타입 제작 |
| 2W 2023.10.30 ~ 2023.11.05 | • 프로토타입 피드백 및 반영<br> • 프로토타입 리팩토링 |
| 3W 2023.11.06 ~ 2023.11.12 | • 데모버전 제작<br> • 데모버전 피드백 |
| 4W 2023.11.13 ~ 2023.11.19 | • 중간 발표 및 피드백 반영<br> • 데모버전 디버깅<br> • 데모버전 리팩토링 |
| 5W 2023.11.20 ~ 2023.11.26 | • 튜토리얼 제작<br> • 영어버전 제작<br> • 밸런스 패치 |
| 6w 2023.11.27 ~ 2023.12.03 | • 알파테스트<br> • FGT(퍼커스그룹테스트)<br> • 밸런스 패치 |
| 7w 2023.12.04 ~ 2023.12.10 | • 베타테스트<br> • 밸런스 패치 |
| 8w 2023.12.11 ~ 2023.12.15 | • 밸런스 패치<br> • 리드미 파일 작성<br> • 최종 발표 |

---

# 3. 기능별 클래스 설명 [🔝](https://github.com/Dream-for-sever-cost/GhostOnly-Removed-paid-assets-#%EB%AA%A9%EC%B0%A8)

- 🌐 **Util** 
    | 클래스명 | 설명 |
    | --- | --- |
    |  |  |
    |  |  |
  
- 🖱️ **UI** [🔝](https://github.com/Dream-for-sever-cost/GhostOnly-Removed-paid-assets-#%EB%AA%A9%EC%B0%A8)
    | 클래스명 | 설명 |
    | --- | --- |
    | UI_Base |  |
    | UI_Popup |  |
  
- 💽 **데이터 관리** [🔝](https://github.com/Dream-for-sever-cost/GhostOnly-Removed-paid-assets-#%EB%AA%A9%EC%B0%A8)
    | 클래스명 | 설명 |
    | --- | --- |
    |  |  |
    |  |  |
  
- **🇺🇲 국제화(I18n)** [🔝](https://github.com/Dream-for-sever-cost/GhostOnly-Removed-paid-assets-#%EB%AA%A9%EC%B0%A8)
    | 클래스명 | 설명 |
    | --- | --- |
    |  |  |
    |  |  |
  
- 🤖 **상태머신** [🔝](https://github.com/Dream-for-sever-cost/GhostOnly-Removed-paid-assets-#%EB%AA%A9%EC%B0%A8)
    | 클래스명 | 설명 |
    | --- | --- |
    |  |  |
    |  |  |
  
- 👻 **유령(플레이어)** [🔝](https://github.com/Dream-for-sever-cost/GhostOnly-Removed-paid-assets-#%EB%AA%A9%EC%B0%A8)
    | 클래스명 | 설명 |
    | --- | --- |
    |  |  |
    |  |  |
  
- 💀 **해골** [🔝](https://github.com/Dream-for-sever-cost/GhostOnly-Removed-paid-assets-#%EB%AA%A9%EC%B0%A8)
    | 클래스명 | 설명 |
    | --- | --- |
    |  |  |
    |  |  |
  
- 🦸 **영웅** [🔝](https://github.com/Dream-for-sever-cost/GhostOnly-Removed-paid-assets-#%EB%AA%A9%EC%B0%A8)
    | 클래스명 | 설명 |
    | --- | --- |
    |  |  |
    |  |  |

- ⚔️ **장비(무기)** [🔝](https://github.com/Dream-for-sever-cost/GhostOnly-Removed-paid-assets-#%EB%AA%A9%EC%B0%A8)
    | 클래스명 | 설명 |
    | --- | --- |
    |  |  |
    |  |  |
  
- 🏛️ **제단** [🔝](https://github.com/Dream-for-sever-cost/GhostOnly-Removed-paid-assets-#%EB%AA%A9%EC%B0%A8)
    | 클래스명 | 설명 |
    | --- | --- |
    |  |  |
    |  |  |
  
- 🪦 **묘비** [🔝](https://github.com/Dream-for-sever-cost/GhostOnly-Removed-paid-assets-#%EB%AA%A9%EC%B0%A8)
    | 클래스명 | 설명 |
    | --- | --- |
    |  |  |
    |  |  |
  
- 📜 **마도서(연구소)** [🔝](https://github.com/Dream-for-sever-cost/GhostOnly-Removed-paid-assets-#%EB%AA%A9%EC%B0%A8)
    | 클래스명 | 설명 |
    | --- | --- |
    |  |  |
    |  |  |
  
- ⚰️ **관(상점)** [🔝](https://github.com/Dream-for-sever-cost/GhostOnly-Removed-paid-assets-#%EB%AA%A9%EC%B0%A8)
    | 클래스명 | 설명 |
    | --- | --- |
    |  |  |
    |  |  |
  
- ‼️ **특성** [🔝](https://github.com/Dream-for-sever-cost/GhostOnly-Removed-paid-assets-#%EB%AA%A9%EC%B0%A8)
    | 클래스명 | 설명 |
    | --- | --- |
    |  |  |
    |  |  |
  
- ❓ **튜토리얼** [🔝](https://github.com/Dream-for-sever-cost/GhostOnly-Removed-paid-assets-#%EB%AA%A9%EC%B0%A8)
    | 클래스명 | 설명 |
    | --- | --- |
    |  |  |
    |  |  |
  
- ⚙️ **설정** [🔝](https://github.com/Dream-for-sever-cost/GhostOnly-Removed-paid-assets-#%EB%AA%A9%EC%B0%A8)
    | 클래스명 | 설명 |
    | --- | --- |
    |  |  |
    |  |  |

---

# 4. 사용한 기술 목록 [🔝](https://github.com/Dream-for-sever-cost/GhostOnly-Removed-paid-assets-#%EB%AA%A9%EC%B0%A8)

- **프레임 워크 & 언어**
    - .Net 2.0
    - C#
- **게임 엔진**
    - Unity - 2022.3.2f1
- **버전 관리**
    - GitHub
- **개발 환경**
    - **IDE**
        - VisualStudio
        - Rider: JetBrains
    - **OS**
        - Window 10
        - MacOS
- **데이터 관리**
    - Google Spreadsheet
- **디자인**
    - piskel
---

# 5. 라이선스 [🔝](https://github.com/Dream-for-sever-cost/GhostOnly-Removed-paid-assets-#%EB%AA%A9%EC%B0%A8)

- **UI Particle**
    - [show-homepage](https://openupm.com/packages/com.coffee.ui-particle/#usage)
    - [show-license](https://github.com/mob-sakai/ParticleEffectForUGUI/blob/4e4b9eb2a756219cf9632525e0acb00aec30aa92/LICENSE.md?plain=1#L1-L7)
    - MIT
- **둥근모 글꼴**
    - [show-homepage](https://neodgm.dalgona.dev/index.html)
    - [show-license](https://github.com/neodgm/neodgm/blob/51aaf6e273a9bd9f9f01d647e65e572da7181128/LICENSE.txt#L1-L97)
    - SIL Open Font License 1.1
