# Prompt Engineering - Part 1 - Duration 30 minutes

> **🎯 DEMO IN 15 MINUTES?** Jump to:
>
> - [⚡ Demo Flow at a Glance](#-demo-flow-at-a-glance-30-minutes) - Your roadmap
> - [📋 Copy-Paste Prompts](#-demo-ready-prompts-copy-paste) - Ready prompts
> - [🚨 Last-Minute Prep](#-last-minute-prep-5-min-before-demo) - Final checklist

---

## 🧭 QUICK NAVIGATION

- [🚨 Last-Minute Prep](#-last-minute-prep-5-min-before-demo) - Start here!
- [📋 Demo Script](#-one-page-demo-script) - Your 30-min roadmap
- [📋 Copy-Paste Prompts](#-demo-ready-prompts-copy-paste) - Ready-to-use prompts
- [🆘 Emergency Backup](#-emergency-backup-plan) - If things go wrong
- [🎬 Q&A Prep](#-post-demo-qa---expected-questions) - Expected questions

---

## ⚡ DEMO FLOW AT A GLANCE (30 Minutes)

| Time | Section | Key Action |
|------|---------|------------|
| 0-2 min | Opening | Hook audience, show of hands |
| 2-7 min | Demo 1 | Bad vs Good Prompt (Specificity) |
| 7-12 min | Demo 2 | Zero-Shot vs Few-Shot |
| 12-17 min | Demo 3 | Chain of Thought |
| 17-23 min | Demo 4 | Role Play + Parameters & Tokenizer |
| 23-25 min | Demo 5 | Iterative Refinement (Live) |
| 25-28 min | Takeaways | 3 Rules to Remember |
| 28-30 min | Q&A | Questions & Resources |

**Format:** Text-only, Azure OpenAI Playground (Chat mode)

**💡 Pro Tip:** Keep this table visible on a second screen or printout!

---

## 🚨 LAST-MINUTE PREP (5 MIN BEFORE DEMO)

### 🔥 YOUR 30-SECOND ELEVATOR PITCH

> "Prompt Engineering is like being a great manager - the clearer your instructions, the better the results. Today, I'll show you how to 10x your AI outputs with simple techniques anyone can learn!"

### ✅ LAST-MINUTE CHECKLIST (2 minutes before go)

- [ ] Playground open & logged in (Chat mode)
- [ ] Tokenizer tab open (platform.openai.com/tokenizer)
- [ ] Prompts copied to clipboard (Bad vs Good, Few-shot, CoT, Role Play, Iterative refinement)
- [ ] Slides in presenter view (agenda + takeaways)
- [ ] Timer set & water ready
- [ ] Internet connection stable
- [ ] Screen share tested
- [ ] Deep breath - you got this! 😊

---

## 📋 ONE-PAGE DEMO SCRIPT

### [0-2 min] OPENING

**Say (energetic):**

> "Good morning — I'm Swamy. Prompt engineering is the skill of telling AI exactly what you want. Today I'll show four text-only techniques you can use in the Playground to get dramatically better results — no code required. Quick show of hands — who's used ChatGPT or Playground before? Great. Let's get started!"

**Transition line:**
> "First: one tiny change that makes a big difference."

### [2-7 min] DEMO 1: Bad vs Good Prompt (Specificity)

**On screen (Playground):** Paste and run:

```text
BAD:
Write about AI
```

Wait for result. Then paste and run:

```text
GOOD:
You are a technical writer for business leaders. 

Write a 200-word introduction to AI focused on practical benefits and ROI. 

Use non-technical language, one short example, and end with one next step they can take.
```

**Say:**
> "Same model — different output. We defined role, audience, length, tone, and format. That's specificity."

**Highlight to audience:** Role, Audience, Length, Tone, Output format

**Transition:**
> "Now let's show how giving *examples* makes the model follow your judgment."

### [7-12 min] DEMO 2: Zero-Shot vs Few-Shot (Examples)

**On screen (Playground):** First run zero-shot:

```text
ZERO-SHOT:
Classify sentiment: "The product works well but delivery was slow"
```

Show result. Then run few-shot (paste whole block):

```text
FEW-SHOT:
Classify customer feedback as: Feature Request | Bug Report | Praise

Examples:
"Love the app but need dark mode" → Feature Request
"Crashes on startup" → Bug Report  
"Best software ever!" → Praise

Now classify:
1) "Can you add export to PDF?"
2) "Amazing customer support!"
3) "Login button doesn't work"
```

**Say:**
> "With examples, the model follows our labels and style. Few-shot is your quickest way to teach model-specific criteria."

**Tip to audience:** Keep examples short, consistent, and representative.

**Transition:**
> "Next — making the model *show its work* for better reasoning."

### [12-17 min] DEMO 3: Chain of Thought (CoT)

**On screen (Playground):** Run first blind prompt:

```text
WITHOUT CoT:
If a store had 50 laptops, sold 15 Monday, 20 Tuesday, received 30 Wednesday, how many do they have now?
```

Show result. Then run Chain-of-Thought prompt (paste):

```text
CHAIN-OF-THOUGHT:
Solve step-by-step. Show your work and calculations.

Problem:
A store had 50 laptops. Sold 15 on Monday, sold 20 on Tuesday, and received 30 on Wednesday. How many laptops are there now?

Steps to show:
1) Starting inventory
2) After Monday sales
3) After Tuesday sales
4) After Wednesday delivery
5) Final answer
```

**Say:**
> "Asking for steps reduces mistakes and clarifies reasoning. Use CoT for arithmetic, logic, or multi-step decisions."

**Quick note:** CoT can be verbose — use only when accuracy matters.

**Transition:**
> "Now a practical demo: role-playing the tone and style you want — and then we'll check tokens and parameters."

### [17-23 min] DEMO 4: Role Play + Parameters & Tokenizer

#### Part A — Role Play (3 minutes)

**On screen (Playground):** Paste and run:

```text
ROLE PLAY:
You are a friendly customer support agent for "TechGadgets Store".

Guidelines:
- Empathetic tone
- Provide three possible next steps (one low-effort, one moderate, one escalated)
- Keep response under 120 words

Customer: "My order hasn't arrived and it's been 2 weeks!"
```

**Say:**
> "Notice how role and explicit guidelines shape the structure and length."

**Demonstration point:** role + rules = predictable style & structure.

#### Part B — Parameters & Tokenizer (3 minutes)

**Show tabs:** Tokenizer (https://platform.openai.com/tokenizer) — paste a short prompt and show token count.

**On screen (Playground):** Paste prompt and point out length vs tokens:

```text
You are an expert summarizer. Summarize the following paragraph in 3 bullets focusing on business impact:

[Paste 2–3 short sentences]
```

**Say:**
> "Tokens ≠ words — you pay per token and models have context limits. Keep prompts efficient: be concise, provide only necessary context, and prefer structured inputs."

**Quick parameter demo in Playground (explain verbally while showing UI):**
- Temperature low (0.2) → predictable answers
- Temperature medium (0.6) → creative variations
- Max tokens control length

**Tip:** Show one small change (temperature) and explain effect quickly.

**Transition:**
> "Finally — a rapid iterative refinement so you see how prompts evolve."

---

### [23-25 min] DEMO 5: Iterative Refinement (Make prompts better live)

**On screen (Playground):** Start with a vague prompt, then refine live:

**V1:**
```text
Write LinkedIn post about our AI workshop.
```

Show result. Then refine:

**V2:**
```text
You are a marketing manager. Write a LinkedIn post (120–150 words) announcing "Prompt Engineering Masterclass" next Saturday. Tone: professional-excited. Include 3 key takeaways and a short registration CTA.
```

Show improved result. Optionally show V3 adding hashtags or audience.

**Say:**
> "Iterate: test → analyze output → add constraints or examples → repeat. Save the best as templates."

**Transition:**
> "Now let's wrap with three practical takeaways."

### [25-28 min] KEY TAKEAWAYS (3 Rules to Remember)

**Display slide & say:**

> **Three Rules**
> 
> 1. **Be Specific** — define role, audience, length, tone.
> 2. **Use Examples** — few-shot teaches model your labels.
> 3. **Iterate** — prompts improve with small, targeted changes.

**Actionable next step:**
> "Pick one task you do daily — create a template prompt for it now. Save it and refine."

### [28-30 min] Q&A + CLOSING

**Say:**
> "Questions? I've got 2 minutes — if more, we can continue in the Meetup chat."

**Prepared quick answers:**
- **Cost:** "Costs depend on model and tokens; monitor usage and start small."
- **Production:** "Yes — use Playground for prototyping; for production, integrate via API and add RAG, logging, and moderation."
- **GPT-3.5 vs GPT-4:** "GPT-4 better at reasoning/longer context; 3.5 cheaper for simple tasks."

**Closing line:**
> "Thank you — happy prompting! Find today's prompts and slides on the GitHub repo and join Global AI Hyderabad."

---

## 🆘 EMERGENCY BACKUP PLAN (Text-Only)

### If Playground is Slow/Down

1. ✅ Show screenshots of expected outputs
2. ✅ Use OpenAI Playground (platform.openai.com) as fallback
3. ✅ Talk through the "BAD → GOOD" examples from slides

### If Internet Fails

1. ✅ Talk through slides and read the "BAD → GOOD" examples aloud
2. ✅ Interactive Q&A session
3. ✅ "Let me show you how it works conceptually..."

### If Audience Seems Lost

1. ✅ Slow down, ask one simple live poll ("Who's used Playground?")
2. ✅ Redo the Bad vs Good demo more slowly
3. ✅ Skip advanced topics, focus on basics
4. ✅ More live interaction, less lecture

---

## 📋 DEMO READY PROMPTS (COPY-PASTE)

### PROMPT 1: The Transformer (Show First!)

```text
BAD ❌:
Generate email

GOOD ✅:
You are a professional email writer.

Task: Write a follow-up email after a sales meeting
Recipient: C-level executive
Tone: Professional, concise, value-focused
Length: 150 words max
Include: Meeting summary, next steps, CTA

Meeting notes: Discussed AI implementation, budget approved, start date Jan 2026
```

### PROMPT 2: The Classifier

```text
FEW-SHOT ✅:
Classify customer feedback as: Feature Request | Bug Report | Praise

Examples:
"Love the app but need dark mode" → Feature Request
"Crashes on startup" → Bug Report  
"Best software ever!" → Praise

Now classify:
1. "Can you add export to PDF?"
2. "Amazing customer support!"
3. "Login button doesn't work"
```

### PROMPT 3: The Reasoner

```text
CHAIN OF THOUGHT ✅:
Solve this step-by-step, showing all work:

Problem: A store had 50 laptops. Monday sold 15, Tuesday sold 20, 
Wednesday received 30 new ones. How many total now?

Steps to show:
1. Starting inventory
2. After Monday sales
3. After Tuesday sales
4. After Wednesday delivery
5. Final count
```

### PROMPT 4: Role Play

```text
ROLE PLAY ✅:
You are a friendly customer support agent for "TechGadgets Store".

Guidelines:
- Empathetic tone
- Provide three possible next steps (one low-effort, one moderate, one escalated)
- Keep response under 120 words

Customer: "My order hasn't arrived and it's been 2 weeks!"
```

### PROMPT 4B: Iterative Refinement

```text
V1 (Vague):
Write LinkedIn post about our AI workshop.

V2 (Refined):
You are a marketing manager. Write a LinkedIn post (120–150 words) announcing "Prompt Engineering Masterclass" next Saturday. Tone: professional-excited. Include 3 key takeaways and a short registration CTA.
```

### PROMPT 5: The Analyzer

```text
DATA ANALYSIS ✅:
Analyze these customer reviews and create insights:

Reviews:
1. "Great product, terrible packaging"
2. "Love it! Fast shipping too"
3. "Product ok, customer service amazing"
4. "Excellent quality, pricey though"
5. "Good but missing features"

Output as markdown table:
| Theme | Mentions | Sentiment | Action Items |
```

---

## 🎬 POST-DEMO Q&A - EXPECTED QUESTIONS

### Q1: "How much does Azure OpenAI cost?"

**A:** "Pricing is per 1000 tokens. GPT-4 is ~$0.03 per 1K tokens. Check Azure pricing calculator for latest rates."

### Q2: "Can we use this in production?"

**A:** "Yes! Azure OpenAI is enterprise-ready with SLA guarantees, data privacy, compliance certifications."

### Q3: "What's the difference between GPT-3.5 and GPT-4?"

**A:** "GPT-4 is more accurate, handles longer contexts (32K vs 4K tokens), but costs ~10x more."

### Q4: "How do I get started?"

**A:** "1. Get Azure subscription 2. Request Azure OpenAI access 3. Deploy a model and experiment!"

### Q5: "Can it replace developers?"

**A:** "No - it makes developers MORE productive. It handles routine tasks, freeing you for creative problem-solving!"

---
